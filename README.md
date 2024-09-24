# OwnLight.UserService Documentation

# Overview

The OwnLight.DeviceService is a crucial component of the OwnLightSystem, a microservice-based architecture developed for the second semester project. This service handles all device-related functionalities, including registration, control, and monitoring of devices such as luminaires. Additionally, it manages the associations between devices and users, enabling seamless control and monitoring within the system.

## Architecture

The OwnLight.DeviceService is designed with a focus on scalability, maintainability, and flexibility, adhering to Domain-Driven Design (DDD) principles. It employs the CQRS (Command Query Responsibility Segregation) pattern to distinctly separate read and write operations, enhancing performance and simplifying the codebase. The microservice architecture ensures modularity, allowing the service to interact seamlessly with other services through well-defined APIs, facilitating future expansion and integration.

## Key Components

- **Controllers**: Handle incoming HTTP requests and route them to the appropriate handlers. They are organized by device functionalities (e.g., registration, control, monitoring).
- **Services**: Centralize business logic, such as device management, message handling, and validation. These services act as intermediaries between controllers and repositories.
- **Repositories**: Manage the database interaction using Entity Framework to perform CRUD operations on device-related data.
- **Entities**: Represent the core domain models like Device and other related domain objects.
- **Mappings**: Use AutoMapper to map between domain entities and DTOs (Data Transfer Objects), simplifying data handling between layers.
- **MediatR Handlers**: Handle the commands and queries defined in the CQRS architecture, decoupling the execution logic from controllers.
- **Middlewares**: Configure and manage middleware components for handling cross-cutting concerns such as logging, authentication, and error handling.
- **Background Services**: Execute long-running tasks or scheduled operations in the background, ensuring the main application remains responsive.
- **Validation**: Implement validation logic to ensure data integrity and enforce business rules before processing requests.
- **DTOs**: Define Data Transfer Objects for API responses and requests, ensuring a clear contract between the API and its consumers.

## Project Structure

The project is organized into multiple layers based on the responsibilities, ensuring a clean separation of concerns:

```
OwnLight.UserService/
├── UserService.API/
│   ├── Controllers/               # Handles HTTP requests (Auth/Admin/User controllers)
│   ├── Middlewares/               # Configuration of the middlewares for the API
│   ├── Program.cs                 # Application startup configuration
│   ├── APIServiceRegistration.cs  # Registers services and dependencies
│   └── Properties/
│       └── appsettings.json       # Application settings and configuration
│
├── UserService.Application/
│   ├── Common/
│   │   ├── Mappings/              # AutoMapper profiles for entity to DTO mappings
│   │   ├── Services/              # Business logic (Auth, Email, Message services)
│   │   └── Validation/            # Validation logic for various operations
│   ├── DTO's/                     # Data Transfer Objects for API responses and requests
│   ├── Features/
│   │   ├── User/                  # Handlers, Commands, Queries related to User
│   │   ├── Admin/                 # Handlers, Commands related to Admin operations
│   │   └── Auth/                  # Handlers, Commands related to Authentication
│   └── ApplicationServiceRegistration.cs  # Registers application services
│
├── UserService.Domain/
│   ├── Entities/                  # Domain entities (User, etc.)
│   ├── Interfaces/                # Domain interfaces (IUserRepository, IAuthRepository)
│   └── Primitives/                # Basic domain concepts and value objects
│
├── UserService.Infrastructure/
│   ├── Data/
│   ├── Background/                # Configuration of the background services
│   ├── Repositories/              # Concrete implementations of the domain repositories
│   └── InfrastructureServiceRegistration.cs  # Registers infrastructure services
│
└── Migrations/                    # Database migrations for setting up and updating the schema
```

## Database Schema

The OwnLight.UserService uses a PostgreSQL database to manage user-related data. Below is the schema for the User table:

### User Table

| Column Name | Data Type | Constraints          |
|-------------|-----------|----------------------|
| Id          | uuid      | Primary Key          |
| Name        | varchar(30) | Not Null           |
| UserName    | varchar(30) | Not Null, Unique   |
| Email       | varchar(255) | Not Null, Unique  |
| Password    | varchar(255) | Not Null          |
| CreatedAt   | timestamp | Default: Utc.Now     |
| UpdatedAt   | timestamp |                      |
| IsLoggedIn  | bool      | Default: false       |
| LastLoginAt | timestamp |                      |

### Description of Columns

- **Id**: A unique identifier for each user.
- **Name**: The full name of the user.
- **UserName**: A unique username for the user, used for login purposes.
- **Email**: The user's email address, which must be unique in the system.
- **Password**: A hashed password for secure authentication.
- **CreatedAt**: Timestamp of when the user was created.
- **UpdatedAt**: Timestamp of the last update to the user's information.
- **IsLoggedIn**: A boolean value indicating whether the user is currently logged in.
- **LastLoginAt**: The timestamp of the user's last login.

## Getting Started

### Pre-requisites

Ensure you have the following installed:

- .NET 8.0 SDK
- PostgreSQL

### Installation

Clone the repository:
```sh
git clone <repository-url>
```

Navigate to the project directory:
```sh
cd OwnLight.UserService
```

Restore dependencies:
```sh
dotnet restore
```

If you wish to make your own migrations (while on infrastructure directory):
```sh
dotnet ef migrations add YourMigration --startup-project ..\UserService.API\
```

Apply database migrations (while on infrastructure directory):
```sh
dotnet ef database update --startup-project ..\UserService.API\
```

Run the service:
```sh
dotnet run .\UserService.API\
```

## Configuration

The OwnLight.UserService is configured using the `appsettings.json` file. Below are some of the key settings:

- **ConnectionStrings**: Defines the connection to the PostgreSQL database.
- **Authentication**: Contains settings related to user login and session management. (still under development)

## API Endpoints

The OwnLight.UserService exposes several endpoints to handle user and admin functionalities:

### User Endpoints:

- `GET /api/users`: Retrieve a list of users.
- `POST /api/users`: Create a new user.
- `GET /api/users/{id}`: Retrieve a user by ID.
- `PUT /api/users/{id}`: Update a user by ID.
- `DELETE /api/users/{id}`: Delete a user by ID.

### Authentication Endpoints:

- `POST /api/auth/login`: Log a user into the system.
- `POST /api/auth/logout`: Log out a user.

### Admin Endpoints:

- `POST /api/admin/delete{"all"}`: Delete all users except for admin (only on development environment)

All responses follow standard REST patterns, returning appropriate HTTP status codes (200, 400, 404, etc.) and messages.

## Contributing

Contributions are welcome! Please follow the standard Git workflow:

1. Fork the repository
2. Create a new feature branch (`git checkout -b feature/my-feature`)
3. Commit your changes (`git commit -am 'Add new feature'`)
4. Push to the branch (`git push origin feature/my-feature`)
5. Open a pull request

## License

This project is licensed under the MIT License. See the LICENSE file for more details.

## Final Thoughts

This API is part of a microservice architecture based project, for my college (FACENS) UPX subject. Later it will be connected to an Ocelot API Gateway, and will be related with other ASP.NET APIs such as:

- `DeviceService`: Responsible for registering and controlling the devices.
- `AutomationService`: Responsible for registering rooms, groups, and schedules for the devices.
- `EnergyService`: Responsible for monitoring the energy costs of all the devices.

The mix of all the microservices, Ocelot Gateway, databases, and the front-end will be our app called **OwnLight**.

Thanks for the attention, and see you on my next projects!

