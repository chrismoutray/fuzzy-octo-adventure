@echo off

C:
cd %windir%\system32\inetsrv

:: where `%~dp0` gives you the current directory that the batch was run from
set currentpath=%~dp0
:: remove trailing slash
if %currentpath:~-1%==\ set currentpath=%currentpath:~0,-1%
:: remove trailing scripts path
set repopath=%currentpath:~0,-8%

:: setup www site
set sitename=www.fullstack.co.uk
set sitepath=%repopath%\FullStack.Web\build
call :SetupSite
call :WriteHostFile

:: setup api site
set sitename=api.fullstack.co.uk
set sitepath=%repopath%\FullStack.WebAPI
call :SetupSite
call :WriteHostFile

goto :eof

:SetupSite
appcmd delete site %sitename%
appcmd delete apppool %sitename%
appcmd add apppool /name:%sitename%  /managedRuntimeVersion:"v4.0" /managedPipelineMode:Integrated
appcmd add site /name:%sitename% /physicalPath:%sitepath% /bindings:http/*:80:%sitename%
appcmd set app /app.name:%sitename%/ /applicationPool:%sitename%
appcmd start site /site.name:%sitename%
echo.
goto :eof

:WriteHostFile
set hostpath=%windir%\system32\drivers\etc\hosts
set entry=127.0.0.1 %sitename%
find /c "%entry%" %hostpath% >nul 2>&1
if %errorlevel% equ 0 goto :eof
echo.>>%hostpath%
echo.%entry%>>%hostpath%
echo.
goto :eof
