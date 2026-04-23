# event-ticketing-backend

> REST API for an event ticket sales platform — focused on real backend challenges like concurrency control, secure authentication, and clean architecture.

---

##  About the Project

Built with ASP.NET Core (.NET 8) following a Controller-based architecture, with a dedicated Service layer for business logic.

This is not a basic CRUD. The project focuses on solving real-world backend problems:

- Preventing double-booking through concurrency control
- Protecting endpoints with JWT-based authentication
- Shielding database entities using the DTO pattern
- Keeping the codebase clean and maintainable as it scales

>  **Active development** — features are being added progressively. See the [Roadmap](#️-roadmap) for current status.

---

##  Features

- **JWT Authentication** — secure login and token generation via a dedicated `TokenService`
- **Events API** — full CRUD for event management
- **DTO Pattern** — `EventCreateDto` and `UserCreateDto` prevent Mass Assignment attacks
- **EF Core Migrations** — database schema managed with Entity Framework Core
- **Swagger UI** — all endpoints documented and testable out of the box

---

##  Tech Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core (.NET 8) |
| Language | C# |
| ORM | Entity Framework Core 8 |
| Database | SQL Server |
| Auth | JWT (JSON Web Tokens) |
| Docs | Swagger / Swashbuckle |

---

##  Project Structure

```
Access.API/
├── Controllers/
│   ├── AuthController.cs       # Registration & login endpoints
│   └── EventsController.cs     # Event CRUD endpoints
├── Data/
│   └── AppDbContext.cs          # EF Core database context
├── DTOs/
│   ├── EventCreateDto.cs
│   ├── EventResponseDto.cs
│   ├── UserCreateDto.cs
│   ├── UserResponseDto.cs
│   ├── UserUpdateDto.cs  
│   └── LoginDto.cs
├── Models/
│   ├── Event.cs
│   └── User.cs
├── Services/
│   ├── ITokenService.cs         # Token service interface
│   ├── TokenService.cs          # JWT generation logic
│   ├── IPasswordService.cs      # Password service interface
│   └── PasswordService.cs       # CreatePasswordHash(hash and salt) and VerifyPassword logic 
├── Migrations/                  # EF Core migration history
└── Program.cs                   # DI container & app configuration
```

---

##  Getting Started

### Prerequisites

- .NET 8 SDK
- SQL Server (local or remote)

### Setup

1. **Clone the repository**

git clone https://github.com/event-ticketing-app/event-ticketing-backend.git
cd event-ticketing-backend

2. **Configure your environment**

Copy the example file and fill in your own values:

cp appsettings.example.json Access.API/appsettings.Development.json

3. **Apply migrations**

dotnet ef database update

4. **Run the API**

dotnet run

Open https://localhost:<port>/swagger to explore the endpoints.
##  API Endpoints

### Auth

| Method | Endpoint | Description | Auth |
|---|---|---|---|
| `POST` | `/api/auth/register` | Register a new user | No |
| `POST` | `/api/auth/login` | Login and receive JWT | No |


### Events

| Method | Endpoint | Description | Auth |
|---|---|---|---|
| `GET` | `/api/events` | List all events | No |
| `POST` | `/api/events` | Create a new event | Yes |
| `PUT` | `/api/events/{id}` | Update an event | Yes |
| `DELETE` | `/api/events/{id}` | Delete an event | Yes |

---

##  Roadmap

- [x] JWT Authentication
- [x] Events CRUD
- [x] DTO pattern (Mass Assignment protection)
- [x] Service layer (TokenService)
- [x] Password hashing (PasswordService)
- [x] User registration endpoint
- [ ] Users CRUD
- [ ] Repository layer
- [ ] Ticket reservation system
- [ ] Concurrency control (double-booking prevention)
- [ ] Role-based access control (Admin / User)
- [ ] Deployment (Azure / Railway)
- [ ] Base de datos en la nube

---

##  Author

**Joel**
- GitHub: [@joeldc-dev](https://github.com/joeldc-dev)
- LinkedIn: [Joel Doña Corral](https://www.linkedin.com/in/joel-doña-corral-6667473b4/)
