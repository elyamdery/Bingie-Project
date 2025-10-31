# Bingie Troubleshooting Guide

## Building and Running
- `dotnet build Bingie/Bingie.csproj -f net8.0-maccatalyst` — Build the Mac Catalyst target without launching.
- `dotnet run --project Bingie/Bingie.csproj -f net8.0-maccatalyst` — Build and attempt to launch the Mac Catalyst app (requires a launch profile or simulator on your machine).

## Diagnostics
- `dotnet test Bingie.sln` — Run the full test suite for app and services.
- `dotnet clean Bingie.sln` — Clear build outputs when you hit stale binaries or launch profile issues.

## Launch Errors
- "The launch profile "(Default)" could not be applied" — Configure a Mac Catalyst launch profile in Visual Studio or pass `--launch-profile` when you run `dotnet run`.
