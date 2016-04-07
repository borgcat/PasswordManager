param(
    [parameter(Mandatory=$true)] [string]$buildConfiguration
)

$workingPath = (new-object system.io.directoryinfo $PSScriptRoot).parent.fullname
$outpath = $workingPath
$project = $workingPath | split-path -leaf

if([string]::IsNullOrWhitespace($buildConfiguration))
{
	$buildConfiguration = "debug"
}	

Write-Host '------------------------'
Write-Host 'Running from ' $workingPath
Write-Host '------------------------'


$globalJson = Get-Content -Path $workingPath\global.json -Raw -ErrorAction Ignore | ConvertFrom-Json -ErrorAction Ignore

if($globalJson)
{
    $dnxVersion = $globalJson.sdk.version
    $dnxRuntime = $globalJson.sdk.runtime
    $dnxArchitecture = $globalJson.sdk.architecture
}

$dnxPath = "{0}\.dnx\runtimes\dnx-{1}-win-{2}.{3}\bin\dnx.exe" -f $env:USERPROFILE, $dnxRuntime, $dnxArchitecture, $dnxVersion

Write-Host '------------------------'
Write-Host 'using this runtime ' $dnxPath
Write-Host '------------------------'

$testRelativePath = $globalJson.projects | Where-Object {$_ -like '*xunit*'}

$testProject = "{0}\{1}" -f $workingPath, $testRelativePath
$OpenCoverUtil = "{0}\src\packages\OpenCover.4.6.519\tools\OpenCover.Console.exe" -f $workingPath
$ReportGeneratorUtil = "{0}\src\packages\ReportGenerator.2.4.4.0\tools\ReportGenerator.exe" -f $workingPath


foreach($projectFile in $globalJson.projects | Where-Object {$_ -like '*xunit*'})
{
    $buildCommands=$buildCommands+$projectFile

    if(Test-Path $testProject)
    {
        Write-Host "$testProject exists for $project"

        cd $testProject
        $projectName = $testProject | split-path -leaf
        $outputXml = "{0}\{1}.coverage.xml" -f $workingPath, $projectName

        $exe = "C:\Program Files (x86)\PowerShell Community Extensions\Pscx3\Pscx\Apps\echoargs.exe"

        &$OpenCoverUtil -register:user -target:"$dnxPath" -targetargs:`"--lib $testProject\bin\$buildConfiguration\dnxcore50 test`" -output:$outputXml -skipautoprops -returntargetcode -filter:`"+[Password*]* -[xunit*]*`" 
				
        cd $workingPath
        &$ReportGeneratorUtil -reports:"$outputXml" -targetdir:"$workingPath\GeneratedReports\ReportGenerator Output"
    }
}


<# 
Write-Host $dnxPath

#$targetArgs = '"--lib {0}\src\PasswordManager.Api\bin\debug\dnxcore50 {0}\src\PasswordManager.Core.Tests.xUnit test"' -f $workingPath
$targetArgs = '"--lib {0}\src\PasswordManager.Core.Tests.xUnit\bin\Debug\dnxcore50 test"' -f $workingPath

Write-Host $targetArgs

$openCover = "{0}\src\packages\OpenCover.4.6.519\tools\OpenCover.Console.exe" -f $workingPath

Write-Host $openCover -target:$dnxPath -targetargs:$targetArgs -output:coverage.xml -filter:+[Src]*

&$openCover -target:$dnxPath -targetargs:$targetArgs -output:coverage.xml -filter:+[Src]* #>