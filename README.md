Here's a `README.md` for your code generator:

---

# .NET 8 3-Tier Architecture Code Generator

This code generator automates the creation of a .NET 8 Web API project based on a 3-tier architecture. It simplifies the setup by generating the necessary layers, configuring essential services, and installing the required packages.

## Table of Contents

- [Overview](#overview)
- [Generated Structure](#generated-structure)
- [Features](#features)
- [Configuration](#configuration)
- [Installation](#installation)
- [Usage](#usage)
- [Packages Installed](#packages-installed)
- [Contributing](#contributing)
- [License](#license)

## Overview

This code generator creates a .NET 8 Web API project following a 3-tier architecture:
- **Presentation Layer** (`App.Api`)
- **Business Layer** (`App.DataService`)
- **Data Layer** (`App.Entities`)

It automatically configures JWT authentication, database connections, identity management, and Swagger, and provides a foundation for implementing repositories, services, and data models.

## Generated Structure

The generator creates the following structure:

```
App.Api/
    - Controllers/
    - Program.cs (with EF, JWT, Swagger, and Identity settings)
    - appsettings.json (with JWT and Database connection configurations)
    
App.DataService/
    - Data/
        - AppDbContext.cs (integrated with EF Core and Identity)
    - Repositories/
        - GenericRepository.cs
        - IGenericRepository.cs
    - Services/
        - EmailSender.cs (implements IEmailSender)
    
App.Entities/
    - Models/
        - AppUser.cs (inherits IdentityUser)
```

## Features

- **3-Tier Architecture**: The generator creates a clean separation of concerns with presentation, business, and data layers.
- **JWT Configuration**: Configures JWT settings in `appsettings.json` and `Program.cs` for authentication and authorization.
- **EF Core Integration**: Automatically sets up Entity Framework Core with database connection settings in `Program.cs`.
- **Identity Management**: Adds `AppUser` class inheriting from `IdentityUser` and configures Identity in `Program.cs`.
- **Swagger Integration**: Configures Swagger for API documentation in `Program.cs`.
- **Generic Repository Pattern**: Implements a generic repository pattern for data access in the `App.DataService` layer.
- **Email Service**: Provides an `EmailSender` class that implements the `IEmailSender` interface for email functionality.
- **Automatic Package Installation**: Installs all necessary packages to make the project functional.

## Configuration

The generator overwrites `appsettings.json` and `Program.cs` with the following configurations:

- **appsettings.json**:
  - JWT Configuration
  - Database Connection Configuration

- **Program.cs**:
  - EF Core Database Connection
  - JWT Settings
  - Identity Settings
  - Swagger Settings

## Installation

To install the code generator, follow these steps:

1. Clone the repository.
2. Run the setup script to install the generator.

```bash
git clone https://github.com/ahmed812003/3-Tier-Architecture-Web-Api-Code-Generator.git
cd <repository-directory>
```

## Usage

After installing the generator, use the following command to generate a new .NET 8 3-tier architecture project:

```bash
dotnet run --project <path-to-generator>
```

This will generate the project structure with all configurations and dependencies in place.

## Packages Installed

The generator automatically installs the following essential packages:

- `Microsoft.AspNetCore.Authentication.JwtBearer`
- `Microsoft.AspNetCore.Identity.EntityFrameworkCore`
- `Microsoft.EntityFrameworkCore`
- `Microsoft.EntityFrameworkCore.Design`
- `Microsoft.EntityFrameworkCore.SqlServer`
- `Microsoft.EntityFrameworkCore.Tools`
- `Microsoft.VisualStudio.Web.CodeGeneration.Design`
- `System.IdentityModel.Tokens.Jwt`

## Contributing

Contributions are welcome! Please fork the repository and submit a pull request for review.
