# CivicHub

**CivicHub** is a platform for **adding**, **searching**, and **connecting persons** with detailed info, supporting **English (en)** and **Georgian (ka)**

## Installation

CivicHub requires [.NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) and [Docker](https://www.docker.com/get-started/)(for integration tests). Ensure both are installed before running the project

## Usage
In order to see localization, **Accept-Language** must be set to **en** or **ka**

## Seed data
During the database update, two cities are automatically added to database
```
{
  Code: "TB",
  Name: "Tbilisi"
}
{
  Code: "BT",
  Name: "Batumi"
}
```

## How to

### Database creation
Replace **CivicHubContext** connection string in **appsettings.json** with your database connection string

Run command from the root of the app
```
dotnet ef database update --project src/CivicHub.Persistance --startup-project src/CivicHub.Api --connection "<Your connection string>"
```
### Testing an api
In order to test apis you'll need to download [Postman](https://www.postman.com/downloads/)
