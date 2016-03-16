if ($env:build_number -eq $null)
{
    "could not find the build number ($env:build_number)" | Write-Error
    Exit 1
}

# load up the global.json so we can find the DNX version
$globalJson = Get-Content -Path $PSScriptRoot\global.json -Raw -ErrorAction Ignore | ConvertFrom-Json -ErrorAction Ignore

Write-Host "===== Updating Verisons ====="
foreach ($project in $globalJson.projects) {
    $parsedJson = Get-Content -Path $PSScriptRoot\$project\project.json -Raw -ErrorAction Ignore | ConvertFrom-Json -ErrorAction Ignore
    $option = [System.StringSplitOptions]::RemoveEmptyEntries

    if($parsedJson)
    {
        $version = $parsedJson.version
        [string[]]$versionArray = $version -split '-'

        if($versionArray.Count -gt 1)
        {
            $newVerion = "{0}-{1}" -f $versionArray[0], $env:build_number
            Write-Host 'update version to: '$newVerion
            $parsedJson.version = $newVerion       
            $parsedJson | ConvertTo-Json | Out-File $PSScriptRoot\$project\project.json
        }    
    }
}


