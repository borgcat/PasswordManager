$branch = "master"
$sourceDir = $PSScriptRoot;
$buildConfiguration = "release";

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

# install DNX
# only installs the default (x86, clr) runtime of the framework.
# If you need additional architectures or runtimes you should add additional calls
# ex: & $env:USERPROFILE\.dnx\bin\dnvm install $dnxVersion -r coreclr
& $env:USERPROFILE\.dnx\bin\dnvm install $dnxVersion -Persistent

# run DNU restore on all project.json files in the src folder including 2>1 to redirect stderr to stdout for badly behaved tools
Write-Host "===== RESTORE <-- done in Prebuild.ps1 ====="
<# foreach ($project in $globalJson.projects) {
    Write-Host 'restoring project: ' $PSScriptRoot\$project\project.json
    & dnu restore $PSScriptRoot\$project\project.json 
}
 #>
 
# run DNU build on all project.json files in the src folder including 2>1 to redirect stderr to stdout for badly behaved tools
Write-Host "===== BUILD ====="
foreach ($project in $globalJson.projects) {
    Write-Host 'Building project: ' $PSScriptRoot\$project\project.json
    & dnu build $PSScriptRoot\$project\project.json --configuration "$buildConfiguration" 
}

# run DNU publish on all project.json files in the src folder including 2>1 to redirect stderr to stdout for badly behaved tools
Write-Host "===== PUBLISH <-- done in postbuild.ps1 ====="
<#
 foreach ($project in $globalJson.projects) {
    Write-Host 'publishing project: ' $PSScriptRoot\$project\project.json
    & dnu publish $PSScriptRoot\$project\project.json --configuration "$buildConfiguration" -o "./pub"
} #>

# workaround for what seems a bug in dnu not copying runtimes
#Write-Host ("Copy runtimes to {0}\approot" -f $sourceDir)
#Copy-Item $env:USERPROFILE\.dnx\runtimes $sourceDir\approot -Recurse

