# fuzzy-octo-adventure

## TODO 

https://github.com/ngbp/ngbp/issues/165#issuecomment-25201730
then move to local.domain and staging.domain and www.domain
then move to local-api.domain and staging-api.domain and api.domain

## Setup

Configure IIS through setup script; which creates sites and app-pools in IIS for web and api. Also creates entries in host file for endpoints www.fullstack.co.uk (AngularJS site) and api.fullstack.co.uk (ASP.NET C# WebAPI site).

`.../scripts/setup-iis.bat` (runas admin)

## FullStack.WebAPI

Build and start solution; `/FullStack.WebAPI/FullStack.WebAPI.sln`

Build with VS2013 Community Edition.

## FullStack.Web

Command Prompt (runas Admin).

```
> npm -g install grunt-cli bower
```

```
> npm -g install karma
```

> The karam package may fail to install if it needs python to do a **node-gyp rebuild**.
> So install python v2 into `C:\Program Files\Python27` https://www.python.org/getit/windows/.
> Then try the following

```
> set PYTHON=C:\Program Files\Python27\python.exe
> npm -g install karma
```

```
> cd FullStack.Web
> npm install
> bower install
> grunt build
```

Naviagte to `http://www.fullstack.co.uk` 

Can use `/scripts/start.bat` to kick off the grunt task and open chrome to the default web site.

Third-party libraries - add using Bower and packages should install  to `vendor/`.

Anything added to this directory will need to be manually added to `build.config.js` and `karma/karma-unit.js` to be picked up by the build system.
