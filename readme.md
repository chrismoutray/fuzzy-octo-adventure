# fuzzy-octo-adventure

## Setup

Configure IIS through setup script; which creates sites and app-pools in IIS for web and api. Also creates entries in host file for endpoints www.fullstack.co.uk (AngularJS site) and api.fullstack.co.uk (ASP.NET C# WebAPI site).

`.../scripts/setup-iis.bat` (runas admin)

## FullStack.WebAPI

Build and start solution; `/FullStack.WebAPI/FullStack.WebAPI.sln`

Build with VS2013 Community Edition.

## FullStack.Web

Command Prompt (runas Admin).
```
> cd FullStack.Web
> npm -g install grunt-cli karma bower
> npm install
> bower install
> grunt build
```

Naviagte to `http://www.fullstack.co.uk` 

Can use `/scripts/start.bat` to kick off the grunt task and open chrome to the default web site.

Third-party libraries - add using Bower and packages should install  to `vendor/`.

Anything added to this directory will need to be manually added to `build.config.js` and `karma/karma-unit.js` to be picked up by the build system.
