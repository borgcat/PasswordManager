[cmdletbinding(SupportsShouldProcess=$true)]
param(
	[parameter(Mandatory=$true)] [string]$project,
    [parameter(Mandatory=$true)] [string]$buildConfiguration
)

$publishLocation = ".publish";

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


try{

	Write-Host 'publishing project: ' $PSScriptRoot\$project\project.json
	& dnu publish $PSScriptRoot\$project\project.json --configuration "$buildConfiguration"  --wwwroot "wwwroot" --wwwroot-out "wwwroot" -o "./" + $publishLocation --iis-command "api" --quiet
}
catch{
    "An error occurred during publish.`n{0}" -f $_.Exception.Message | Write-Error
}
$iisApp = "./" + $publishLocation

  & C:\Program Files (x86)\IIS\Microsoft Web Deploy V3\msdeploy.exe" -source:IisApp=$iisApp -dest:IisApp='sogeti-api-passwordmanager',ComputerName='https://sogeti-api-passwordmanager.scm.azurewebsites.net/msdeploy.axd',UserName='$sogeti-api-passwordmanager',Password='GiLeFFXJhiCsymLaMclq95jo7uDvx9wxRgg3yB7jk2s4NpQSYBrfpcbjolrE',IncludeAcls='False',AuthType='Basic' -verb:sync -enableLink:contentLibExtension  -retryAttempts:2