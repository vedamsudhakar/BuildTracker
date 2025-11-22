# Build Tracker API Documentation

## Overview
This API allows external build scripts to automatically insert build information into the Build Tracker system.

## Base URL
```
http://localhost:5253/api/builds
```

## Endpoints

### 1. Create Build
**POST** `/api/builds`

Creates a new build record in the system.

#### Request Body
```json
{
  "buildType": 0,
  "buildPath": "\\\\server\\builds\\MyApp_v1.0.0.zip",
  "releaseNotes": "Bug fixes. Performance improvements.",
  "date": "2025-11-21T20:00:00",
  "version": "1.0.0",
  "ftpServerId": 1
}
```

#### Field Descriptions
- `buildType` (required): Integer representing build type
  - `0` = Development
  - `1` = QA
  - `2` = Staging
  - `3` = Production
- `buildPath` (required): Network path or location of the build
- `releaseNotes` (optional): Release notes or changelog
- `date` (optional): Build date/time (defaults to current time if not provided)
- `version` (required): Version string (e.g., "1.0.0")
- `ftpServerId` (optional): ID of associated FTP server

#### Response (201 Created)
```json
{
  "id": 123,
  "buildType": 0,
  "buildPath": "\\\\server\\builds\\MyApp_v1.0.0.zip",
  "releaseNotes": "Bug fixes. Performance improvements.",
  "date": "2025-11-21T20:00:00",
  "version": "1.0.0",
  "ftpServerId": 1,
  "ftpServer": null
}
```

### 2. Get Build by ID
**GET** `/api/builds/{id}`

Retrieves a specific build record.

#### Response (200 OK)
```json
{
  "id": 123,
  "buildType": 0,
  "buildPath": "\\\\server\\builds\\MyApp_v1.0.0.zip",
  "releaseNotes": "Bug fixes. Performance improvements.",
  "date": "2025-11-21T20:00:00",
  "version": "1.0.0",
  "ftpServerId": 1,
  "ftpServer": {
    "id": 1,
    "name": "Production FTP",
    "host": "ftp.example.com",
    "port": 21,
    "username": "ftpuser",
    "password": "******",
    "isActive": true
  }
}
```

### 3. Get All Builds
**GET** `/api/builds`

Retrieves all build records, ordered by date (newest first).

#### Response (200 OK)
```json
[
  {
    "id": 123,
    "buildType": 0,
    "buildPath": "\\\\server\\builds\\MyApp_v1.0.0.zip",
    "releaseNotes": "Bug fixes. Performance improvements.",
    "date": "2025-11-21T20:00:00",
    "version": "1.0.0",
    "ftpServerId": 1,
    "ftpServer": { ... }
  },
  ...
]
```

## Usage Examples

### PowerShell Script
```powershell
# Build script example
$buildInfo = @{
    buildType = 0  # Development
    buildPath = "\\\\buildserver\\releases\\MyApp_v1.2.3.zip"
    releaseNotes = "Fixed login bug. Added new dashboard."
    version = "1.2.3"
    ftpServerId = 1
} | ConvertTo-Json

$headers = @{
    "Content-Type" = "application/json"
}

Invoke-RestMethod -Uri "http://localhost:5253/api/builds" `
    -Method Post `
    -Body $buildInfo `
    -Headers $headers
```

### cURL
```bash
curl -X POST http://localhost:5253/api/builds \
  -H "Content-Type: application/json" \
  -d '{
    "buildType": 3,
    "buildPath": "\\\\server\\builds\\MyApp_v2.0.0.zip",
    "releaseNotes": "Major release with new features",
    "version": "2.0.0",
    "ftpServerId": 2
  }'
```

### Python
```python
import requests
import json
from datetime import datetime

build_data = {
    "buildType": 1,  # QA
    "buildPath": "\\\\buildserver\\qa\\MyApp_v1.5.0.zip",
    "releaseNotes": "QA build for testing",
    "date": datetime.now().isoformat(),
    "version": "1.5.0",
    "ftpServerId": 1
}

response = requests.post(
    "http://localhost:5253/api/builds",
    json=build_data,
    headers={"Content-Type": "application/json"}
)

print(f"Build created with ID: {response.json()['id']}")
```

## Error Responses

### 400 Bad Request
Returned when validation fails.
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "BuildPath": ["The BuildPath field is required."],
    "Version": ["The Version field is required."]
  }
}
```

### 404 Not Found
Returned when a build with the specified ID doesn't exist.
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.4",
  "title": "Not Found",
  "status": 404
}
```

## Notes
- The API currently does **not** require authentication. Consider adding API key authentication for production use.
- All dates should be in ISO 8601 format.
- The `ftpServerId` must reference an existing FTP server in the database.
