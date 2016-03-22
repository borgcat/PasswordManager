[cmdletbinding(SupportsShouldProcess=$true)]
param(
    [parameter(Mandatory=$true)] [string]$publishProfile,
    [parameter(Mandatory=$true)] [string]$azurePassword
)

$profileJson = "{0}\{1}" -f $PSScriptRoot , $publishProfile
$publishLocation = 'publish'

Write-Host '----------------------------------------'
Write-Host $profileJson
Write-Host '----------------------------------------'


# load up the global.json so we can find the DNX version
$parsedJson = Get-Content -Path $profileJson -Raw -ErrorAction Ignore | ConvertFrom-Json -ErrorAction Ignore

if($parsedJson)
{
    $project = $parsedJson.project
    $buildConfiguration = $parsedJson.buildConfiguration

    $azureAppSite = $parsedJson.azureAppSite
    $azureComputerName = $parsedJson.azureComputerName
    $azureUserName = $parsedJson.azureUserName
	$publishRuntime =  $parsedJson.azurePublishRuntime
	$publishIISCommand = $parsedJson.iisCommand
}

Write-Host '----------------------------------------'
Write-Host 'source project: ' $project
Write-Host 'build configuration: ' $buildConfiguration
Write-Host 'published dest: ' $azureAppSite
Write-Host 'published computername: ' $azureComputerName
Write-Host 'published username: ' $azureUserName
Write-Host 'published runtime: ' $publishRuntime
Write-Host 'published iis-command: ' $publishIISCommand
Write-Host '----------------------------------------'
Write-Host

$publishLocation = "publish";

# load up the global.json so we can find the DNX version
$globalJson = Get-Content -Path $PSScriptRoot\global.json -Raw -ErrorAction Ignore | ConvertFrom-Json -ErrorAction Ignore

if($globalJson)
{
    $dnxVersion = $globalJson.sdk.version
    $dnxRuntime = $globalJson.sdk.runtime
	$dnxArchitecture = $globalJson.sdk.architecture
}
else
{
    Write-Warning "Unable to locate global.json to determine using 'latest'"
    $dnxVersion = "latest"
}

Write-Host '----------------------------------------'
$dnxPath = "{0}\.dnx\runtimes\dnx-{1}-win-{2}.{3}\bin\dnx.exe" -f $env:USERPROFILE, $dnxRuntime, $dnxArchitecture, $dnxVersion
$displaydnx = "{0} from {1}" -f $publishRuntime, $profileJson
Write-Host $dnxPath ' from global.json'
Write-Host $displaydnx
Write-Host '----------------------------------------'

& $env:USERPROFILE\.dnx\bin\dnvm use $dnxVersion -r coreclr -Persistent
$iisApp = "{0}\.{1}" -f $PSScriptRoot, $publishLocation
$iisDeployApp = "{0}\wwwroot" -f $iisApp

Write-Host 'published location: ' $iisApp

try{
	#run DNU restore on project.json files in the src folder
	Write-Host "===== RESTORE <-- done in Prebuild.ps1 ====="
	Write-Host 'restoring project: ' $PSScriptRoot\$project\project.json
	& dnu restore $PSScriptRoot\$project\project.json 
	
	Write-Host 'publishing project: ' $PSScriptRoot\$project\project.json
	& dnu publish $PSScriptRoot\$project\project.json --configuration "$buildConfiguration"  --wwwroot "wwwroot" --wwwroot-out "wwwroot" -o $iisApp --iis-command $publishIISCommand --runtime $publishRuntime
    $msDeploy = "C:\Program Files (x86)\IIS\Microsoft Web Deploy V3\msdeploy.exe"
    $iisDestProvider = "IisApp='{0}',ComputerName='{1}',UserName='{2}',Password='{3}',IncludeAcls='False',AuthType='Basic'" -f $azureAppSite, $azureComputerName, $azureUserName, $azurePassword
        
	Write-Host "===== DEPLOY  ====="	
	&$msDeploy -source:IisApp=$iisDeployApp -dest:$iisDestProvider -verb:sync -enableLink:contentLibExtension -allowUntrusted -enableRule:DoNotDeleteRule -retryAttempts:2
}
catch{
    "An error occurred during publish.`n{0}" -f $_.Exception.Message | Write-Error
     ##teamcity[buildStatus status='FAILURE']
    [System.Environment]::Exit(1)
}




