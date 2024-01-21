@echo off
set ASPNETCORE_ENVIRONMENT=Development
start "WebServer" cmd /k "cd .\TradingAlpha.App\ && dotnet run --launch-profile http"
start http://localhost:5105