# OwnLight.UserService Documentation

# Overview

The OwnLight.DeviceService is a component of the OwnLightSystem (a microservice-based system), developed as part of the second semester project. It is responsible for managing all device-related functionalities, such as registration, control, and monitoring of devices (like luminaires) within the system. This service also manages associations between devices and users for control and monitoring purposes.

## Architecture

The OwnLight.DeviceService is built following Domain-Driven Design (DDD) principles and uses a CQRS (Command Query Responsibility Segregation) pattern to separate reading and writing operations. The architecture is designed to ensure scalability, maintainability, and flexibility for future expansion. The microservice architecture promotes modularity, and the service interacts with other services via well-defined APIs.

## Key Components

- **Controllers**: Handle incoming HTTP requests and route them to the appropriate handlers. They are organized by device functionalities (e.g., registration, control, monitoring).
- **Services**: Centralize business logic, such as device management, message handling, and validation. These services act as intermediaries between controllers and repositories.
- **Repositories**: Manage the database interaction using Entity Framework to perform CRUD operations on device-related data.
- **Entities**: Represent the core domain models like Device and other related domain objects.
- **Mappings**: Use AutoMapper to map between domain entities and DTOs (Data Transfer Objects), simplifying data handling between layers.
- **MediatR Handlers**: Handle the commands and queries defined in the CQRS architecture, decoupling the execution logic from controllers.

## Project Structure

The project is organized into multiple layers based on the responsibilities, ensuring a clean separation of concerns:

```
OwnLight.DeviceService/
├── DeviceService.API/
│   ├── Controllers/               # Handles HTTP requests (Device controllers)
│   ├── Middlewares/               # Configuration of the middlewares for the API
│   ├── Program.cs                 # Application startup configuration
│   ├── APIServiceRegistration.cs  # Registers services and dependencies
│   └── Properties/
│       └── appsettings.json       # Application settings and configuration
│
├── DeviceService.Application/
│   ├── Common/
│   │   ├── Mappings/              # AutoMapper profiles for entity to DTO mappings
│   │   ├── Services/              # Business logic (Device management, Message services)
│   │   └── Validation/            # Validation logic for various operations
│   ├── DTO's/                     # Data Transfer Objects for API responses and requests
│   ├── Features/
│   │   ├── Device/                # Handlers, Commands, Queries related to Device
│   └── ApplicationServiceRegistration.cs  # Registers application services
│
├── DeviceService.Domain/
│   ├── Entities/                  # Domain entities (Device, etc.)
│   ├── Interfaces/                # Domain interfaces (IDeviceRepository)
│   └── Primitives/                # Basic domain concepts and value objects
│
├── DeviceService.Infrastructure/
│   ├── Data/
│   ├── Background/                # Configuration of the background services
│   ├── Repositories/              # Concrete implementations of the domain repositories
│   └── InfrastructureServiceRegistration.cs  # Registers infrastructure services
│
└── Migrations/                    # Database migrations for setting up and updating the schema
```

## Database Schema

The OwnLight.DeviceService uses a PostgreSQL database to manage device-related data. Below are the schemas for the Device, DeviceAction, and DeviceType tables:

### Device Table

| Column Name | Data Type   | Constraints          |
|-------------|-------------|----------------------|
| Id          | uuid        | Primary Key          |
| Name        | varchar(30) | Not Null             |
| TypeId      | uuid        | Foreign Key          |
| Status      | varchar(30) | Not Null             |
| CreatedAt   | timestamp   | Default: UtcNow      |
| UpdatedAt   | timestamp   |                      |
| UserId      | uuid        | Foreign Key          |

### DeviceAction Table

| Column Name | Data Type   | Constraints          |
|-------------|-------------|----------------------|
| Id          | uuid        | Primary Key          |
| DeviceId    | uuid        | Foreign Key          |
| Action      | varchar(30) | Not Null             |
| Timestamp   | timestamp   | Default: UtcNow      |

### DeviceType Table

| Column Name | Data Type   | Constraints          |
|-------------|-------------|----------------------|
| Id          | uuid        | Primary Key          |
| TypeName    | varchar(30) | Not Null             |
| Description | text        |                      |

### Description of Columns

- **Device Table**:
    - **Id**: A unique identifier for each device.
    - **Name**: The name of the device.
    - **TypeId**: The unique identifier for the type of the device.
    - **Status**: The current status of the device (e.g., active, inactive).
    - **CreatedAt**: Timestamp of when the device was created.
    - **UpdatedAt**: Timestamp of the last update to the device's information.
    - **UserId**: A unique identifier for the user associated with the device.

- **DeviceAction Table**:
    - **Id**: A unique identifier for each action.
    - **DeviceId**: A unique identifier for the device associated with the action.
    - **Action**: The action performed on the device (e.g., turn on, turn off).
    - **Timestamp**: Timestamp of when the action was performed.

- **DeviceType Table**:
    - **Id**: A unique identifier for each device type.
    - **TypeName**: The name of the device type.
    - **Description**: A description of the device type.

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
cd OwnLight.DeviceService
```

Restore dependencies:
```sh
dotnet restore
```

If you wish to make your own migrations (while on infrastructure directory):
```sh
dotnet ef migrations add YourMigration --startup-project ..\DeviceService.API\
```

Apply database migrations (while on infrastructure directory):
```sh
dotnet ef database update --startup-project ..\DeviceService.API\
```

Run the service:
```sh
dotnet run .\DeviceService.API\
```

## Configuration

The OwnLight.DeviceService is configured using the `appsettings.json` file. Below are some of the key settings:

- **ConnectionStrings**: Defines the connection to the PostgreSQL database.
- **DeviceSettings**: Contains settings related to device management and monitoring.

## API Endpoints

The OwnLight.DeviceService exposes several endpoints to handle device functionalities:

### Device Endpoints:

- `GET /api/devices`: Retrieve a list of devices.
- `POST /api/devices`: Create a new device.
- `GET /api/devices/{id}`: Retrieve a device by ID.
- `PUT /api/devices/{id}`: Update a device by ID.
- `DELETE /api/devices/{id}`: Delete a device by ID.

### Monitoring Endpoints:

- `GET /api/devices/{id}/status`: Retrieve the status of a device.
- `POST /api/devices/{id}/control`: Control a device (e.g., turn on/off).

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

This API is part of a microservice architecture-based project for my college (FACENS) UPX subject. Later it will be connected to an Ocelot API Gateway and will be related to other ASP.NET APIs such as:

- `UserService`: Responsible for managing user-related functionalities.
- `AutomationService`: Responsible for registering rooms, groups, and schedules for the devices.
- `EnergyService`: Responsible for monitoring the energy costs of all the devices.

The mix of all the microservices, Ocelot Gateway, databases, and the front-end will be our app called **OwnLight**.

Thanks for the attention, and see you on my next projects!
