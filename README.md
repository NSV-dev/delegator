# delegator

delegator is a client-server task managing desktop application for groups or companies

## Architecture

![image](https://user-images.githubusercontent.com/69851710/205447506-5bacd82f-0d13-407f-a390-bc86ec13446c.png)

Contains 5 layers: MSSQL database, API Library (Connection to DB + Models), API (ASP.NET Core), UI Library (Connection to API + Models), UI (WPF). Used SOLID, DRY principles. Used MVC for API, DAO for API Lib, MVVM for UI, Facade for UI Lib.

## Patterns and Principles

- MVVM for client
- MVC for API
- SOLID
- DRY

### You can download and run this app 

Download this [ZIP](delegator.zip) file and run `setup.exe`

P.S. Currently not working (servers are not free)
