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
}

Write-Host '----------------------------------------'
Write-Host 'source project: ' $project
Write-Host 'build configuration: ' $buildConfiguration
Write-Host 'published dest: ' $azureAppSite
Write-Host 'published computername: ' $azureComputerName
Write-Host 'published username: ' $azureUserName
Write-Host '----------------------------------------'
Write-Host


$publishLocation = "publish";

# load up the global.json so we can find the DNX version
$globalJson = Get-Content -Path $PSScriptRoot\global.json -Raw -ErrorAction Ignore | ConvertFrom-Json -ErrorAction Ignore

if($globalJson)
{
    $dnxVersion = $globalJson.sdk.version
}
else
{
    Write-Warning "Unable to locate global.json to determine using 'latest'"
    $dnxVersion = "latest"
}
& $env:USERPROFILE\.dnx\bin\dnvm use $dnxVersion -r coreclr -Persistent
$iisApp = "{0}\.{1}" -f $PSScriptRoot, $publishLocation
$iisDeployApp = "{0}\wwwroot" -f $iisApp

Write-Host 'published location: ' $iisApp

try{

	Write-Host 'publishing project: ' $PSScriptRoot\$project\project.json
	& dnu publish $PSScriptRoot\$project\project.json --configuration "$buildConfiguration"  --wwwroot "wwwroot" --wwwroot-out "wwwroot" -o $iisApp --iis-command "api" --quiet
    $msDeploy = "C:\Program Files (x86)\IIS\Microsoft Web Deploy V3\msdeploy.exe"
    $iisDestProvider = "IisApp='{0}',ComputerName='{1}',UserName='{2}',Password='{3}',IncludeAcls='False',AuthType='Basic'" -f $azureAppSite, $azureComputerName, $azureUserName, $azurePassword
    
    &$msDeploy -source:IisApp=$iisDeployApp -dest:$iisDestProvider -verb:sync -enableLink:contentLibExtension  -retryAttempts:2
}
catch{
    "An error occurred during publish.`n{0}" -f $_.Exception.Message | Write-Error
    exit 1
}




