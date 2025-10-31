# Bingie Troubleshooting Guide

## Building and Running
- `dotnet build Bingie/Bingie.csproj -f net8.0-maccatalyst` — Build the Mac Catalyst target without launching.
- `dotnet run --project Bingie/Bingie.csproj -f net8.0-maccatalyst` — Build and attempt to launch the Mac Catalyst app (requires a valid launch profile/simulator on your machine).

## Diagnostics
- `dotnet test Bingie.sln` — Run the full test suite for both app and service layers.
- `dotnet test tests/Bingie.Tests/Bingie.Tests.csproj -v minimal` — Quick pass for the focused auth end-to-end specs.
- `dotnet clean Bingie.sln` — Clear build outputs if you observe stale binaries or launch profile errors.

Update this guide whenever we add new workflows or debugging tips.
