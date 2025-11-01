# Maze (.NET 8)

This repository has been migrated from .NET Framework 4.0 to modern SDK-style projects targeting .NET 8.

## Build

Use the .NET SDK (8.0+) to restore and build:

```
# Build the whole solution
 dotnet build Maze.sln -c Release

# Or build just the Maze app project
 dotnet build Maze/Maze.csproj -c Release
```

## Run

Run the console app from the project directory:

```
 dotnet run --project Maze/Maze.csproj
```

Notes:
- Newtonsoft.Json is now referenced via PackageReference (v13.x).
- AssemblyInfo attributes are preserved; csproj disables auto-generation.
- Legacy WebRequest APIs are still used and may be obsolete; consider migrating to HttpClient in a follow-up.
