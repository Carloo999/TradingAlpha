@echo off
cd .\TradingAlpha.App\
dotnet ef database update
cd .. 
launch.bat