# BuildTracker

BuildTracker is a web application designed to streamline the tracking and management of software builds. It provides a centralized platform for teams to log build details, manage release notes with rich text support, and facilitate easy access to build artifacts via FTP.

## Project Goal

The primary goal of BuildTracker is to improve visibility and accessibility of software builds within an organization. It allows teams to:
*   **Track Build History**: Maintain a comprehensive log of all builds, including versions, dates, and types (2D, 3D, Weld Inspect).
*   **Manage Release Notes**: Create detailed, formatted release notes using a rich text editor (Summernote), supporting images, lists, and styling.
*   **Simplify Access**: Integrate with FTP servers to allow direct downloading of build artifacts from the dashboard.
*   **Secure Access**: Implement role-based authentication (Admin/User) to control access to sensitive operations.

## Features

*   **Build Management**: CRUD operations for build information.
*   **Rich Text Editor**: Integrated Summernote editor for Release Notes with image paste support.
*   **FTP Server Management**: Configure and manage multiple FTP server connections.
*   **Direct Downloads**: Download build files directly from the configured FTP servers.
*   **Search & Filtering**: Quickly find builds by path, version, or content in release notes.
*   **User Management**: Admin interface for managing users and roles.

## Setup Details

### Prerequisites

*   [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
*   SQL Server (LocalDB or Standard)

### Configuration

1.  **Clone the repository** (if applicable).
2.  **Database Connection**:
    *   The application uses `appsettings.json` for configuration.
    *   For development, connection strings are securely stored in `appsettings.Development.json`.
    *   Update the `BuildTrackerContext` connection string in `appsettings.Development.json` to point to your SQL Server instance.

    ```json
    "ConnectionStrings": {
      "BuildTrackerContext": "Server=(localdb)\\mssqllocaldb;Database=BuildTrackerContext;Trusted_Connection=True;MultipleActiveResultSets=true"
    }
    ```

### Running the Application

1.  **Apply Migrations**:
    Initialize the database by applying existing migrations.
    ```bash
    dotnet ef database update
    ```

2.  **Run the App**:
    ```bash
    dotnet run
    ```

3.  **Access**:
    Open your browser and navigate to `http://localhost:5253` (or the port indicated in the console).

### Deployment

To publish the application for deployment:

```bash
dotnet publish -c Release -o publish --self-contained false
```

*   **Note**: The published `appsettings.json` may contain placeholder connection strings. Ensure you configure the production environment with the correct credentials.