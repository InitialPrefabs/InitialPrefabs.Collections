$dotnetCmd = 'dotnet test --collect:"XPlat Code Coverage" --settings .\InitialPrefabs.Collections.Tests\coverlet.runsettings'
$reportGeneratorExe = 'reportgenerator.exe'
$targetDir = 'coverageresults'

Write-Host "Running tests and collecting coverage..."
$output = & cmd /c $dotnetCmd 2>&1

$output | ForEach-Object { Write-Host $_ }

$attachmentLine = $output | Where-Object { $_ -match 'Attachments:' }

if ($attachmentLine) {
    $index = [array]::IndexOf($output, $attachmentLine)
    $nextLine = $output[$index + 1]

    if ($nextLine -match '([A-Z]:\\[^\r\n]+\.xml)') {
        $coveragePath = $Matches[1]
        Write-Host "`n✅ Coverage file found:`n$coveragePath`n"

        Write-Host "Generating HTML report using ReportGenerator..."
        & $reportGeneratorExe -reports:"$coveragePath" -targetdir:"$targetDir" -reporttypes:Html

        if ($LASTEXITCODE -eq 0) {
            Write-Host "`nReport generated successfully in: $targetDir"
        } else {
            Write-Warning "ReportGenerator failed with exit code $LASTEXITCODE"
        }
    }
    else {
        Write-Warning "Could not parse coverage file path."
    }
}
else {
    Write-Warning "No 'Attachments:' line found in test output."
}
