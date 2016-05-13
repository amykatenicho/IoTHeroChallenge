@echo off
echo.
echo =======================================================================
echo Uninstall Visual Studio Code Snippets for the module Intro to Azure IoT
echo =======================================================================
echo.

for /f "tokens=2,*" %%a in ('reg query "HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders" /v "Personal" 2^>NUL ^| findstr Personal') do set MyDocuments=%%b

DEL "%MyDocuments%\Visual Studio 2015\Code Snippets\Visual C#\My Code Snippets\AzureIoT*.snippet" 2>NUL
DEL "%MyDocuments%\Visual Studio 2015\Code Snippets\XML\My Xml Snippets\AzureIoT*.snippet" 2>NUL

echo Module Code Snippets have been removed!
PAUSE