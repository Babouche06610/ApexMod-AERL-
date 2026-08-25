$ErrorActionPreference = "Stop"
Write-Host "AERL — restore" -ForegroundColor Cyan
dotnet restore .\AERL.sln
Write-Host "AERL — build" -ForegroundColor Cyan
dotnet build .\AERL.sln -c Debug --no-restore
Write-Host "Build complete." -ForegroundColor Cyan
