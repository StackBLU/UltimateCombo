[xml]$proj = Get-Content "UltimateCombo.csproj"
$assemblyVersion = $proj.Project.PropertyGroup | Where-Object { $_.AssemblyVersion } | Select-Object -First 1 | ForEach-Object { $_.AssemblyVersion }
if ($assemblyVersion) {
    $jsonArray = Get-Content "..\pluginmaster.json" | ConvertFrom-Json
    $jsonArray[0].AssemblyVersion = $assemblyVersion
    "[$($jsonArray | ConvertTo-Json -Depth 10)]" | Set-Content "..\pluginmaster.json"
    Write-Host "Synced version $assemblyVersion to pluginmaster.json"
} else {
    Write-Warning "Could not find AssemblyVersion in project file"
}