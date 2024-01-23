@echo off

set /p "email=Enter valid email: "
set /p "password=Enter valid password (at least 8 characters, including at least one uppercase letter, one lowercase letter, and one digit): "

echo If you did not enter valid credentials the context seed will fails
echo Look at the launch logs for info regarding the context seed
echo {"OwnerCredentials":{"Email":"%email%","Password":"%password%"}} > .\TradingAlpha.App\credentials.json

set /p "alpacaKey=Enter your Alpaca-Api-Key: "
set /p "alpacaSecret=Enter your Alpaca-Api-Secret: "
echo {"ApiSettings":{"AlpacaKey":"%alpacaKey%","AlpacaSecret":"%alpacaSecret%"}} > .\TradingAlpha.App\apisettings.json

cd .\TradingAlpha.App\
dotnet ef database update
cd .. 
launch.bat


