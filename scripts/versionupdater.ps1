$env:build_number = 1234
$parsedJson = Get-Content -Path C:\code\GitHub\Sogeti\backup\project.json -Raw -ErrorAction Ignore | ConvertFrom-Json -ErrorAction Ignore

$option = [System.StringSplitOptions]::RemoveEmptyEntries

if($parsedJson)
{
    $version = $parsedJson.version
    $versionArray = $version.Split("{-}", $option)

    if($versionArray.Count > 1)
    {
        $newVerion = "{0}\.{1}" -f $versionArray[0], $env:build_number
        Write-Host 'old verison: '$version
        Write-Host 'new verison: '$newVerion
    }    
}