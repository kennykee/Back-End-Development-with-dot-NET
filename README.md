# Back-End Development with .NET WebAPI

This repository contains a tutorial project for building a back-end API using ASP.NET Core (.NET 9). It demonstrates the basics of setting up, configuring, and running a WebAPI service.

## Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- Visual Studio or VS Code
- Windows, macOS, or Linux

## Getting Started

1. **Clone the repository:**

   ```sh
   git clone https://github.com/kennykee/Back-End-Development-with-dot-NET.git
   cd Back-End-Development-with-dot-NET
   ```

2. **Restore dependencies:**

   ```sh
   dotnet restore
   ```

3. **Run the application:**

   ```sh
   dotnet run
   ```

   The API will start on the default port (usually 5000 or 5001).

4. **Test the API:**
   - Use tools like [Postman](https://www.postman.com/) or [curl](https://curl.se/) to send HTTP requests.
   - You can also use the built-in Swagger UI at `http://localhost:5000/swagger` (if enabled).

## Project Structure

- `Program.cs` — Main entry point for the WebAPI application.
- `appsettings.json` — Configuration file for application settings.
- `Back-End-Development-with-dot-NET.csproj` — Project file.
- `Properties/launchSettings.json` — Launch configuration for development.

## Useful Commands

- `dotnet build` — Build the project.
- `dotnet test` — Run tests (if available).
- `dotnet publish` — Publish for deployment.

## Additional Resources

- [ASP.NET Core Documentation](https://docs.microsoft.com/aspnet/core)
- [Learn .NET](https://dotnet.microsoft.com/learn)

## License

This project is for educational purposes.
