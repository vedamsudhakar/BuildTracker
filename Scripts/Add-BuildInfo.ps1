# Add-BuildInfo.ps1
# PowerShell script to add build information to Build Tracker

param(
    [Parameter(Mandatory=$true)]
    [ValidateSet("Development", "QA", "Staging", "Production")]
    [string]$BuildType,
    
    [Parameter(Mandatory=$true)]
    [string]$BuildPath,
    
    [Parameter(Mandatory=$true)]
    [string]$Version,
    
    [Parameter(Mandatory=$false)]
    [string]$ReleaseNotes = "",
    
    [Parameter(Mandatory=$false)]
    [int]$FtpServerId,
    
    [Parameter(Mandatory=$false)]
    [string]$ApiUrl = "http://localhost:5253/api/builds"
)

# Map build type string to integer
$buildTypeMap = @{
    "Development" = 0
    "QA" = 1
    "Staging" = 2
    "Production" = 3
}

# Create the build info object
$buildInfo = @{
    buildType = $buildTypeMap[$BuildType]
    buildPath = $BuildPath
    releaseNotes = $ReleaseNotes
    version = $Version
}

# Add FTP server ID if provided
if ($FtpServerId) {
    $buildInfo.ftpServerId = $FtpServerId
}

# Convert to JSON
$jsonBody = $buildInfo | ConvertTo-Json

# Set headers
$headers = @{
    "Content-Type" = "application/json"
}

try {
    Write-Host "Adding build information to Build Tracker..." -ForegroundColor Cyan
    Write-Host "Build Type: $BuildType" -ForegroundColor Gray
    Write-Host "Build Path: $BuildPath" -ForegroundColor Gray
    Write-Host "Version: $Version" -ForegroundColor Gray
    
    # Make the API call
    $response = Invoke-RestMethod -Uri $ApiUrl -Method Post -Body $jsonBody -Headers $headers
    
    Write-Host "`nBuild successfully added!" -ForegroundColor Green
    Write-Host "Build ID: $($response.id)" -ForegroundColor Green
    Write-Host "Date: $($response.date)" -ForegroundColor Gray
    
    return $response
}
catch {
    Write-Host "`nError adding build information:" -ForegroundColor Red
    Write-Host $_.Exception.Message -ForegroundColor Red
    
    if ($_.ErrorDetails.Message) {
        Write-Host "`nDetails:" -ForegroundColor Yellow
        Write-Host $_.ErrorDetails.Message -ForegroundColor Yellow
    }
    
    exit 1
}
