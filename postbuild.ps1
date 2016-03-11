$buildConfiguration = "release";
$publishLocation = ".publish";

 # bootstrap DNVM into this session.
#&{$branch;iex ((new-object net.webclient).DownloadString('https://raw.githubusercontent.com/aspnet/Home/dev/dnvminstall.ps1'))}

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
#& $env:USERPROFILE\.dnx\bin\dnvm install $dnxVersion -Persistent
& $env:USERPROFILE\.dnx\bin\dnvm use $dnxVersion -r coreclr -Persistent

<#
# run DNU publish on all project.json files in the src folder 
Write-Host "===== PUBLISH ====="
foreach ($project in $globalJson.projects) {
    Write-Host 'publishing project: ' $PSScriptRoot\$project\project.json
	& dnu publish $PSScriptRoot\$project\project.json --configuration "$buildConfiguration"  --wwwroot "wwwroot/dist" --wwwroot-out "wwwroot" -o "./" + $publishLocation
} #>
$sourceDir = $PSScriptRoot + "\" + $publishLocation;
Write-Host "Source Dir for Web.Config: " $sourceDir

# update web.config
Write-Host ("Updating {0}\wwwroot\web.config" -f $sourceDir)
Write-Host ("Setting dnx-version to {0}" -f $globalJson.sdk.version)
Write-Host ("Setting dnx-clr to {0}" -f $globalJson.sdk.runtime)
$webConfig = $sourceDir + "\wwwroot\web.config"
Write-Host "Web.Config: " $webConfig
$doc = New-Object System.Xml.XmlDocument
$doc.Load($webConfig)
$doc.SelectSingleNode('configuration/appSettings/add[@key="dnx-version"]').Attributes['value'].Value = $globalJson.sdk.version
$doc.SelectSingleNode('configuration/appSettings/add[@key="dnx-clr"]').Attributes['value'].Value = $globalJson.sdk.runtime
$doc.Save($webConfig)
Write-Host "web.config updated."
