
# taskt
taskt (formerly sharpRPA) is the first truly free, easy to use, and open source process automation client built on the .NET Framework in C#.  taskt allows you to build a digital workforce and automate straight-forward and repetative actions with ease.  Included is a "what you see is what you get" designer with dozens of automation commands and options as well as a screen recorder that records and replays scripted automation. With taskt, you can spend less time writing code and more time building functionality.

![Main App Window](https://i.imgur.com/ynlr3p6.png)

## How does taskt work?
taskt works by essentially building and executing XML command instructions.  Each automation command has an independent definition and parameters that are required for command execution.  Bot Developers chain the commands together using a rich user interface to create an automation script.

## What can taskt do?
taskt can perform automation on both web and desktop applications, simulating the actions a person would do. Taskt can start and stop processes, launch VB and PowerShell scripts, work directly with Excel workbooks, and perform OCR (OneNote installation required) among many other functions.  You can review all the automation commands by clicking [here](https://github.com/saucepleez/taskt/wiki/Automation-Commands).

## What does a completed task look like?
Depicted below is an example of a completed task (script). Note that scripts can be simple or they can be complex, its all up to you!
![Sample](https://i.imgur.com/fbi8JrB.png)

## Documentation
Please check out the [Wiki](https://github.com/saucepleez/taskt/wiki) for basic documenation surrounding the application and the available commands

## Getting Started For Script Building
- Click [HERE](https://github.com/saucepleez/taskt/releases/download/1.0.0.0/taskt.v1.0.0.0.OFFICIAL-SIGNED.zip) to download the latest from the releases page and extract the taskt project to any folder.  Double-click 'taskt.exe' to launch the application. Note, until the code signing certificate becomes trusted, you may need to click-through SmartScreen.
- Taskt will ask if you want to create a scripts folder under \My Documents\taskt\My Scripts (optional)
- Copy sample files from the taskt project folder to \My Documents\taskt\My Scripts (optional)

## Dependencies
- Excel Commands require Microsoft Excel to be installed and configured.
- OCR Command requires Microsoft One Note to be installed and configured.
- Web Browser Command requires Selenium `chromedriver.exe` to function properly.

## License
This project is licensed under the Apache License - see the LICENSE.md file for details.  This project is free for personal or commercial use.

## Releases and Updates
Officially-signed builds are released generally once per week assuming there are updates.  Each officially-signed build contains all updated features since the last officially signed release.  Otherwise, source code is constantly updated 'daily' as required and is the always the latest.

Version Number Strategy (as of 1.1.0.0):
Major - Breaking Changes or Incompatability
Minor - Features and Enhancement Implementation
Revision - Bugfixes, Post Implementation fixes
Build - Reserved for Rebuilds

If you would like an officially-signed build of a specific or latest version, please [create a new issue.](https://github.com/saucepleez/taskt/issues/new)

## Known Issues
- Screen Recording requires Windows scale settings to be at 100%.

### Support? Questions? Feature Request?
 [Create a new issue!](https://github.com/saucepleez/taskt/issues/new)
 
 [Chat with us on Gitter!](https://gitter.im/taskt-rpa/Lobby)
