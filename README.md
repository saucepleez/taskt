
# taskt
taskt is the first truly free, easy to use, and open source robotic process automation client built on the .NET Framework in C#.  taskt allows you to build a digital workforce and automate straight-forward and repetative actions with ease.  Included is a "what you see is what you get" designer with dozens of automation commands and options as well as a screen recorder that records and replays scripted automation. With taskt, you can spend less time writing code and more time building functionality.

![Main App Window](https://i.imgur.com/ynlr3p6.png)

## How does taskt work?
taskt works by essentially building and executing XML command instructions.  Each automation command has an independent definition and parameters that are required for command execution.  Bot Developers chain the commands together using a rich user interface to create an automation script.

## What can taskt do?
taskt can perform automation on both web and desktop applications. Taskt can start and stop processes, launch VB and PowerShell scripts, work directly with Excel workbooks, and perform OCR (OneNote installation required) among many other functions.  You can review all the automation commands by clicking [here](https://github.com/saucepleez/taskt/wiki/Automation-Commands).

## What does a completed script look like?
Here is a sample script
![Sample](https://i.imgur.com/fbi8JrB.png)

## Documentation
Please check out the [Wiki](https://github.com/saucepleez/taskt/wiki) for basic documenation surrounding the application and the available commands

## Getting Started For Script Building
- Download the latest from the releases page and extract the taskt project to any folder.  Double-click 'taskt.exe' to launch the application.
- Taskt will ask if you want to create a scripts folder under \My Documents\taskt\My Scripts (optional)
- Copy sample files from the taskt project folder to \My Documents\taskt\My Scripts (optional)

## Dependencies
- Excel Commands require Microsoft Excel to be installed and configured.
- OCR Command requires Microsoft One Note to be installed and configured.
- Web Browser Command requires Selenium `chromedriver.exe` to function properly.

## License
This project is licensed under the Apache License - see the LICENSE.md file for details.  This project is free for personal or commercial use.

### Request a Feature
Don't see an automation feature you are looking for or have a great idea for an enhancement?  [Create a new enhancement issue!](https://github.com/saucepleez/taskt/issues/new)
