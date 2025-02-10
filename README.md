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
