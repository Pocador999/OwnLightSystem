# OwnLight.UserService Documentation

# Overview

The OwnLight.UserService is a component of the OwnLightSystem (a microservice based system), developed as part of the second semester project. It is responsible for managing all user-related functionalities, such as registration, authentication, role-based authorization, and profile management within the system. This service also manages associations between users and devices (like luminaires) for control and monitoring purposes.

## Architecture

The OwnLight.UserService is built following Domain-Driven Design (DDD) principles and uses a CQRS (Command Query Responsibility Segregation) pattern to separate reading and writing operations. The architecture is designed to ensure scalability, maintainability, and flexibility for future expansion. The microservice architecture promotes modularity, and the service interacts with other services via well-defined APIs.

## Key Components

- **Controllers**: Handle incoming HTTP requests and route them to the appropriate handlers. They are organized by user roles and functionalities (e.g., authentication, user management, admin operations).
- **Services**: Centralize business logic, such as authentication, message handling, email services, and validation. These services act as intermediaries between controllers and repositories.
- **Repositories**: Manage the database interaction using Entity Framework to perform CRUD operations on user-related data, including user authentication and role management.
- **Entities**: Represent the core domain models like User and other related domain objects.
- **Mappings**: Use AutoMapper to map between domain entities and DTOs (Data Transfer Objects), simplifying data handling between layers.
- **MediatR Handlers**: Handle the commands and queries defined in the CQRS architecture, decoupling the execution logic from controllers.

## Project Structure

The project is organized into multiple layers based on the responsibilities, ensuring a clean separation of concerns:

```
OwnLight.UserService/
├── UserService.API/
│   ├── Controllers/               # Handles HTTP requests (Auth/Admin/User controllers)
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
│   ├── Data/                      # Database context (DbContext)
│   ├── Repositories/              # Concrete implementations of the domain repositories
│   └── InfrastructureServiceRegistration.cs  # Registers infrastructure services
│
└── Migrations/                    # Database migrations for setting up and updating the schema
```

## Getting Started

### Prerequisites

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


### Final Thoughts:

This api is part of a microservice architecture based project, for my college (FACENS) upx subject
later it will be conected to a Ocelot API Gateway, and will be related with other asp.net api's such as:

- `DeviceService`: Responsible for registering and controlling the devices
- `AutomationService`: Responsible for registering rooms, groups, and schedules for the devices 
- `EnergyService`: Responsible for monitoring the energy costs of all the devices

the mix of all the microservices, Ocelot Gateway, Databases and the front-end will be our app called **OwnLight** 

Thanks for the attention, and see you on my next projects!
