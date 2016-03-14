param(
    [parameter(Mandatory=$true)] [string]$project,
    [parameter(Mandatory=$true)] [string]$buildConfiguration
)

Write-Host '----------------------------------------'
Write-Host '       Parameters'
Write-Host '----------------------------------------'
Write-Host $PSScriptRoot
Write-Host '----------------------------------------'

 # bootstrap DNVM into this session.
&{$branch;iex ((new-object net.webclient).DownloadString('https://raw.githubusercontent.com/aspnet/Home/dev/dnvminstall.ps1'))}

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

& $env:USERPROFILE\.dnx\bin\dnvm install $dnxVersion -Persistent

# run DNU restore on all project.json files in the src folder including 2>1 to redirect stderr to stdout for badly behaved tools
Write-Host "===== RESTORE <-- done in Prebuild.ps1 ====="
Write-Host 'restoring project: ' $PSScriptRoot\$project\project.json
& dnu restore $PSScriptRoot\$project\project.json 
	
# run DNU build on all project.json files in the src folder including 2>1 to redirect stderr to stdout for badly behaved tools
Write-Host "===== BUILD ====="
Write-Host 'Building project: ' $PSScriptRoot\$project\project.json
& dnu build $PSScriptRoot\$project\project.json --configuration "$buildConfiguration" 
