:: visit the site for more info 
:: https://github.com/ngbp/ngbp

cd ../FullStack.Web

:: build for development
start grunt watch

:: pause for 5 seconds
ping -n 5 localhost >NUL

set path=%~dp0

start "" "C:\Program Files (x86)\Google\Chrome\Application\chrome.exe" http://www.fullstack.co.uk

:: use default grunt task to build and compile for production
:: $ grunt
