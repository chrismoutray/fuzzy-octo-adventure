# fuzzy-octo-adventure

## FullStack.WebAPI

Build with VS2013 Community Edition.

Build and start solution; `FullStack.WebAPI.sln`

## FullStack.Web

Project based on ngBoilerplate https://github.com/ngbp/ngbp

Command Prompt (runas Admin).

```
> cd FullStack.Web
> npm -g install grunt-cli karma bower
> npm install
> bower install
> start.bat
```

Third-party libraries - add using Bower and packages should install  to `vendor/`.

Anything added to this directory will need to be manually added to `build.config.js` and `karma/karma-unit.js` to be picked up by the build system.