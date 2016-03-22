$workingPath = (new-object system.io.directoryinfo $PSScriptRoot).parent.fullname

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
 foreach ($project in $globalJson.projects) {
    Write-Host 'restoring project: ' $workingPath\$project\project.json
    & dnu restore $workingPath\$project\project.json 
}


$dnxPath = "{0}\.dnx\runtimes\dnx-{1}-win-{2}.{3}\bin\dnx.exe" -f $env:USERPROFILE, $dnxRuntime, $dnxArchitecture, $dnxVersion
Write-Host $dnxPath

#$targetArgs = '"--lib {0}\src\PasswordManager.Api\bin\debug\dnxcore50 {0}\src\PasswordManager.Core.Tests.xUnit test"' -f $workingPath
$targetArgs = '"--lib {0}\src\PasswordManager.Core.Tests.xUnit\bin\Debug\dnxcore50 test"' -f $workingPath

Write-Host $targetArgs

$openCover = "{0}\src\packages\OpenCover.4.6.519\tools\OpenCover.Console.exe" -f $workingPath

Write-Host $openCover -target:$dnxPath -targetargs:$targetArgs -output:coverage.xml -filter:+[Src]*

&$openCover -target:$dnxPath -targetargs:$targetArgs -output:coverage.xml -filter:+[Src]*