@ECHO off
%~d0

CD "%~dp0"
ECHO Install Visual Studio 2015 Code Snippets for the module: Intro to Azure IoT
ECHO ---------------------------------------------------------------------------
CALL .\Scripts\InstallCodeSnippets.cmd
ECHO Done!
ECHO.
ECHO ***************************************************************************
ECHO.

@PAUSE
