# Example-BuildScript.ps1
# Example build script that creates a build and registers it with Build Tracker

# Build configuration
$projectName = "MyApplication"
$buildConfiguration = "Release"
$outputPath = "\\buildserver\releases"

# Get version from file or generate
$version = "1.2.3"  # You can read this from a version file or git tag

# Perform your build steps here
Write-Host "Building $projectName version $version..." -ForegroundColor Cyan

# Example: Build a .NET project
# dotnet build -c $buildConfiguration
# dotnet publish -c $buildConfiguration -o "$outputPath\$projectName_v$version"

# Create build package
$buildFileName = "${projectName}_v${version}.zip"
$buildPath = Join-Path $outputPath $buildFileName

# Example: Create a zip file
# Compress-Archive -Path ".\bin\$buildConfiguration\*" -DestinationPath $buildPath

Write-Host "Build created at: $buildPath" -ForegroundColor Green

# Prepare release notes
$releaseNotes = @"
Fixed login authentication bug.
Added new dashboard features.
Performance improvements.
"@

# Register build with Build Tracker
Write-Host "`nRegistering build with Build Tracker..." -ForegroundColor Cyan

$buildInfo = @{
    buildType = 0  # 0=Development, 1=QA, 2=Staging, 3=Production
    buildPath = $buildPath
    releaseNotes = $releaseNotes
    version = $version
    ftpServerId = 1  # Optional: ID of FTP server if applicable
} | ConvertTo-Json

$headers = @{
    "Content-Type" = "application/json"
}

try {
    $response = Invoke-RestMethod -Uri "http://localhost:5253/api/builds" `
        -Method Post `
        -Body $buildInfo `
        -Headers $headers
    
    Write-Host "Build registered successfully!" -ForegroundColor Green
    Write-Host "Build ID: $($response.id)" -ForegroundColor Gray
    Write-Host "View at: http://localhost:5253/Builds/Details/$($response.id)" -ForegroundColor Gray
}
catch {
    Write-Host "Failed to register build with Build Tracker:" -ForegroundColor Red
    Write-Host $_.Exception.Message -ForegroundColor Red
    # Continue even if registration fails
}

Write-Host "`nBuild process completed!" -ForegroundColor Green
