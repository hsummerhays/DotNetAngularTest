# run.ps1
# This script starts the .NET Backend and Angular Frontend in a single Windows Terminal split window.

# Force PowerShell to use UTF-8 for console output to ensure emojis display correctly
[Console]::OutputEncoding = [System.Text.Encoding]::UTF8

# Get absolute paths and escape spaces if any
$apiPath = (Resolve-Path "src/AwsApp.API").Path
$webPath = (Resolve-Path "src/frontend").Path

# Use Unicode escape sequences for emojis to avoid encoding issues in the script file itself
$rocket = [char]::ConvertFromUtf32(0x1F680)
$check = [char]::ConvertFromUtf32(0x2705)
$globe = [char]::ConvertFromUtf32(0x1F310)

Write-Host "$rocket Launching Clean Architecture AWS App in split terminal..." -ForegroundColor Cyan

# Construct a single argument string for Windows Terminal (wt.exe)
$wtCommand = "nt -p `"Windows PowerShell`" -d `"$apiPath`" powershell -NoExit -Command `"dotnet run`" ; split-pane -H -p `"Windows PowerShell`" -d `"$webPath`" powershell -NoExit -Command `"npm start`""

try {
    Start-Process wt -ArgumentList $wtCommand -ErrorAction Stop
    Write-Host "`n$check Both services are starting in a split Windows Terminal window!" -ForegroundColor Green
}
catch {
    Write-Warning "Windows Terminal (wt.exe) not found. Falling back to separate windows..."
    
    # Fallback to separate windows
    Start-Process powershell -WorkingDirectory $apiPath -ArgumentList "-NoExit", "-Command", "dotnet run"
    Start-Process powershell -WorkingDirectory $webPath -ArgumentList "-NoExit", "-Command", "npm start"
    
    Write-Host "`n$check Both services are starting in separate windows!" -ForegroundColor Green
}

# 3. Wait and Open Browser
Write-Host "`n$globe Waiting for services to initialize before opening browser..." -ForegroundColor Yellow

# Wait for 5 seconds to allow services to start up
Start-Sleep -Seconds 5

# Open Backend (Scalar) FIRST
Write-Host "Opening API: http://localhost:5067/scalar/v1" -ForegroundColor Gray
Start-Process "http://localhost:5067/scalar/v1"

# Small delay between tabs
Start-Sleep -Seconds 1

# Open Frontend SECOND
Write-Host "Opening Frontend: http://localhost:4200" -ForegroundColor Gray
Start-Process "http://localhost:4200"
