#-- Public Module Functions --#

function Invoke-TeamCitySpecFlowReport {
<#
.SYNOPSIS
Produces a report that can be shown in TeamCity.

.PARAMETER Categories
NUnit categories to execute
#>
param(
	[String[]]$Categories = @()
)
	
	try {
		Write-LogMessage $tsr.Messages.Begin		
	
		Remove-FileCache
		
		Invoke-NUnit $Categories
		Invoke-SpecFlow
		
		Publish-Artifacts		
		Add-FileCache
		
		Write-LogMessage $tsr.Messages.End
	
	} catch {
		throw $_.Exception
	}
}

function Set-Properties {
<#
.SYNOPSIS
Updates one or more properties with new values.

.PARAMETER Properties
The properties to set via @{ Key=Value; Key2=Value2 }.

#>
	param(
		[HashTable]$Properties = @{}
	)

	foreach ($key in $Properties.keys) {
		
		$value = $Properties.$key		
		$tsr[$key] = $value
		
		Write-LogMessage ("[TeamCity.SpecFlow.Reporting]: property [{0}] was updated with value [{1}]" -f $key, $value)
    }
}

#-- Private Module Functions --#

function Write-LogMessage{
	[CmdletBinding()]
	param(
		[Parameter(Position=0, ValueFromPipeline=$true)]
		[String]$Message
	)
	
	# TODO: we should streamline the use of write-verbose and write-debug instead
	if($tsr.Verbose) {
		Write-Host $Message
	}
}

function Write-TeamCityServiceMessage {
	param(
		[String]$MessageName,
		[String]$Value
	)
	# ##teamcity[<messageName> 'value']
	# ##teamcity[<messageName> name1='value1' name2='value2']
	
	$message = "##teamcity[{0} '{1}']" -f $MessageName, $Value
	Write-Host $message
}

function Invoke-SpecFlow {
	[CmdletBinding()]
	param()
	
	Write-LogMessage "=== Invoking specflow.exe ==="
	
	if($tsr.PathToSpecFlowExe) {
		$exe = $tsr.PathToSpecFlowExe
	} else {
		$exe = $(Get-SpecFlowPackage).Exe
	}	

	if(Test-Path $exe -PathType:Leaf) {
		try {	
			$ProjectInformation = Get-Project -ea:Stop
		
			$arguments = @()
			
			#https://github.com/techtalk/SpecFlow/wiki/Reporting
			$arguments += $tsr.SpecFlowReportType
			$arguments += $ProjectInformation.PathToProjectFile
			
			$args += '/out:{0}' -f 'TestResult.html'
			
			$source = Get-FirstOrDefault '.\specflow.exe.config'
			if($source) {
				$destination = Split-Path -Parent $exe
				
				if((Test-Path (Join-Path $destination 'specflow.exe.config')) -eq $false) {
					Write-LogMessage $("copying file 'specflow.exe.config' to [{0}]" -f $destination)
					$specflow_exe_config = Copy-Item -Path $source.FullName -Destination $destination -PassThru
				}
			}
			
			Write-Host $exe $arguments -ea:Stop
			Invoke-Executable $exe $arguments -ea:Stop
			
			if($specflow_exe_config) {
				Delete-File $specflow_exe_config.FullName
			}
			
			# TODO: the name of the file needs to be configurable
			$html_report = Get-FirstOrDefault '.\TestResult.html'
			if($html_report) {
				$tsr.GeneratedFiles.HtmlReport = $html_report
			} else {
				Write-Error "SpecFlow 'HtmlReport' output is missing"
			}
			
			
		} catch {
			throw "An error occurred while executing specflow.exe: " + $_
		} 
	} else {
		throw "failed to find 'specflow.exe' at location '$exe'"
	}
}

function Invoke-NUnit {
	[CmdletBinding()]
	param (
		[String[]] $Categories = @()
	)
	Write-LogMessage "=== Invoking nunit.console.exe ==="
	
	if($tsr.PathToNUnitConsoleExe) {
		$exe = $tsr.PathToNUnitConsoleExe
	} else {
		$exe = $(Get-NUnitPackage).Exe
	}
	
	if(Test-Path $exe -PathType:Leaf) {
		try {
			$arguments = @()
			
			$ProjectInformation = Get-Project -ea:Stop
			
			if($ProjectInformation.PathToAssembly) {
				$arguments += $ProjectInformation.PathToAssembly
			} else {
				$arguments += $ProjectInformation.PathToProjectFile
			}
			$arguments += "--labels=All"
			$arguments += "--out=TestResult.txt"
			#$arguments += "--xml=TestResult.xml"
			if($Categories) {
				$arguments += "/include:$($Categories -join '|')"
			}
			
			Write-Host 'executing: ' $exe $arguments -ea:Stop
			Invoke-Executable $exe $arguments -ea:Stop
			
			$output = Get-FirstOrDefault .\TestResult.txt
			$result = Get-FirstOrDefault .\TestResult.xml
			if($output) {
				$tsr.GeneratedFiles.NUnitOutput = $output
			} else {
				Write-Error "NUnit output is missing"				
			}
			
			if($result) {
				$tsr.GeneratedFiles.NUnitReport = $result
			} else {
				Write-Error "NUnit report is missing"
			}	
			
		} catch {
			throw "error when executing nunit-console.exe: " + $_
		}
	} else {
		throw "failed to find 'nunit-console.exe' at location '$exe'"
	}	
}

function Invoke-Executable {
	[CmdletBinding()]
	param(
		[Parameter(Position=0)]
		[String]$Executable,
		[Parameter(Position=1)]
		[String[]]$Parameters
	)
	
	Write-Verbose $("Invoking '{0}'" -f $Executable)
	$Parameters | % {Write-Verbose $_} -Begin {Write-Verbose $("using arguments:")}
	
	& $Executable $Parameters | Out-String | Write-LogMessage
}

function Get-Project {
	[CmdletBinding()]
	param()
	
	$project_file = Get-FirstOrDefault '.\*.*proj'
	if($project_file -eq $null){
		Write-Error "Failed to find the project file"
		return
	}
	
	$out = @{PathToProjectFile = $project_file.FullName}
	
	$assembly_name = '{0}.dll' -f $project_file.BaseName	
	$assembly_path = 'bin' | Join-Path -ChildPath $tsr.Configuration | Join-Path -ChildPath $assembly_name
	
	$assembly_file = Get-FirstOrDefault $(Join-Path . $assembly_path)
	if($assembly_file) {
		$out.PathToAssembly = $assembly_file.FullName
	}	

	return $out;
}

function Get-NUnitPackage {
	
	$tools = Join-Path $(Get-Package "NUnit.Runners").Path 'tools'	
	$nunit_console_exe = Get-Item $(Join-Path $tools 'nunit3-console.exe')

	return @{
		Exe = $nunit_console_exe.FullName
	}
}

function Get-SpecFlowPackage {
	
	$tools = Join-Path $(Get-Package "SpecFlow").Path 'tools'	
	$specflow_exe = Get-Item $(Join-Path $tools 'specflow.exe')

	return @{
		Exe = $specflow_exe.FullName		
	}
}

function Get-PackagesFolder {
	[CmdletBinding()]
	param()
	
	if( (Split-Path $tsr.PathToPackagesFolder -Leaf) -eq 'Packages' ){
		$PackagesFolderPath = $tsr.PathToPackagesFolder
	} else {
		$PackagesFolderPath = Join-Path $tsr.PathToPackagesFolder 'Packages'
	}	
	
	$result = Get-FirstOrDefault $PackagesFolderPath
	if($result -eq $null) {
		$msg = "Failed to find the 'packages' folder at location: '{0}'." -f $PackagesFolderPath
		Write-Error $msg
		return
	}
	
	return $result
}

function Get-Package {
	[CmdletBinding()]
	param(
		[string]$Name
	)
	
	$PackagesFolder = Get-PackagesFolder
	$results = Get-ChildItem $PackagesFolder | Where-Object {
		$_ -match "^($Name){1}(\.)?(\d+\.{0,1})*$"
	}
	
	if($results -eq $null) {
		Write-Error $("failed to find package '{0}' at location: '{1}'" -f $Name, $PackagesFolder.FullName)
		return
	}
	
	$results | ForEach-Object {
		Write-Debug $_.Name
	} -Begin {Write-Debug $("found {0} packages:" -f $results.Count)}
	
	#TODO: handle package versions (this only gets the latest one)
	$package = $results | Sort-Object -Property Name -Descending | Select-Object -First 1
	$version = $package.Name -split "$Name."
	
	$result = @{
		Version = if($version.Length -gt 1) {$version[1]} else {"n/a"}
		Path = $package.FullName
	}
	
	Write-Debug $("package: {0}; version: {1}" -f $result.Path, $result.Version)
	
	return $result

}

function Remove-FileCache{	
	
	if($tsr.CleanEnvironment) {
		
		$x = Get-Content .\files.generated -WarningAction:SilentlyContinue -ErrorAction:SilentlyContinue
		if($x) {
			$x -split ';' | % { Delete-File $_ }
		}
	}
}

function Add-FileCache {
	$($tsr.GeneratedFiles.GetEnumerator() |  % { 
		$_.Value.FullName}) -join ';' | 
		Set-Content .\files.generated | Out-Null
}

function Get-FirstOrDefault {
	param(
		[String]$Path
	)
	
	return Get-Item -Path $Path -ErrorAction:SilentlyContinue | Select-Object -First 1	
}

function Publish-Artifacts {
	
	if($tsr.GeneratedFiles.HtmlReport -and $tsr.PublishArtifacts) {
		Write-TeamCityServiceMessage 'publishArtifacts' $tsr.GeneratedFiles.HtmlReport.FullName		
	}
}

function Delete-File {
	[CmdletBinding()]
	param(
		[String]$Path
	)
	
	if($Path) {
		if(Test-Path $Path -PathType:Leaf) {
			Write-LogMessage $("deleting file [{0}]" -f $Path)
			Remove-Item $Path -ErrorAction:SilentlyContinue | Out-Null
		}
	}
}

function Set-PSConsole {
	try {
		$max = $host.UI.RawUI.MaxPhysicalWindowSize
		if($max) {
		$host.UI.RawUI.BufferSize = New-Object System.Management.Automation.Host.Size(9999,9999)
		$host.UI.RawUI.WindowSize = New-Object System.Management.Automation.Host.Size($max.Width,$max.Height)
	}
	} catch {}

}

# default values
# override by Set-Properties @{Key=Value} outside of this script
$tsr = @{
	Configuration = 'Release'
	SpecFlowReportType = 'nunitexecutionreport'
	PublishArtifacts = $true
	CleanEnvironment = $true
	PathToPackagesFolder = '..\'
	Verbose = $true
	Messages = @{
		Begin = "##teamcity[blockOpened name='TeamCity.SpecFlow.Reporting']"
		End = "##teamcity[blockClosed name='TeamCity.SpecFlow.Reporting']"
	}
	GeneratedFiles = @{}
	Indentation = 0
	PathToNUnitConsoleExe = ''	
}

if ($env:TEAMCITY_VERSION) {
	Set-PSConsole
}
Export-ModuleMember -Function Invoke-TeamCitySpecFlowReport, Set-Properties


