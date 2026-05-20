# event-ticketing-backend

> REST API for an event ticket sales platform — focused on real backend challenges like concurrency control, secure authentication, and clean architecture.

---

## About the Project

Built with ASP.NET Core (.NET 8) following a Controller-based architecture, with a dedicated Service layer for business logic.

This is not a basic CRUD. The project focuses on solving real-world backend problems:

- Preventing double-booking through concurrency control
- Protecting endpoints with JWT-based authentication
- Shielding database entities using the DTO pattern
- Keeping the codebase clean and maintainable as it scales

> **Active development** — features are being added progressively. See the [Roadmap](#roadmap) for current status.

---

## Features

- **JWT Authentication** — secure login and token generation via a dedicated `TokenService`
- **Events API** — full CRUD for event management
- **Users API** — full CRUD for user management at admin level
- **Ticket Reservation System** — reserve and purchase flow with 10-minute expiration control
- **Service Layer** — `TokenService`, `PasswordService` and `TicketService` isolate business logic from controllers
- **Password Hashing** — secure registration using HMACSHA512 salt and hash via a dedicated `PasswordService`
- **DTO Pattern** — input and output DTOs prevent Mass Assignment attacks and decouple the API contract from the database schema
- **EF Core Migrations** — database schema managed with Entity Framework Core
- **Swagger UI** — all endpoints documented and testable out of the box

---

## Tech Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core (.NET 8) |
| Language | C# |
| ORM | Entity Framework Core 8 |
| Database | SQL Server |
| Auth | JWT (JSON Web Tokens) |
| Docs | Swagger / Swashbuckle |

---

## Project Structure

```
Access.API/
├── Controllers/
│   ├── AuthController.cs        # Registration & login endpoints
│   ├── EventsController.cs      # Event CRUD endpoints
│   └── UsersController.cs       # User management endpoints
├── Data/
│   └── AppDbContext.cs           # EF Core database context
├── DTOs/
│   ├── EventCreateDto.cs
│   ├── EventResponseDto.cs
│   ├── UserCreateDto.cs
│   ├── UserResponseDto.cs
│   ├── UserUpdateDto.cs
│   ├── LoginDto.cs
│   ├── TicketReserveResponseDto.cs
│   └── TicketPurchaseResponseDto.cs
├── Enums/
│   └── TicketStatus.cs           # Reserved, Purchased, Cancelled
├── Models/
│   ├── Event.cs
│   ├── User.cs
│   └── Ticket.cs
├── Services/
│   ├── ITokenService.cs          # Token service interface
│   ├── TokenService.cs           # JWT generation logic
│   ├── IPasswordService.cs       # Password service interface
│   ├── PasswordService.cs        # CreatePasswordHash and VerifyPassword logic
│   ├── ITicketService.cs         # Ticket service interface
│   └── TicketService.cs          # Reservation and purchase logic
├── Migrations/                   # EF Core migration history
└── Program.cs                    # DI container & app configuration
```

---

## Getting Started

### Prerequisites

- .NET 8 SDK
- SQL Server (local or remote)

### Setup

1. **Clone the repository**

```
git clone https://github.com/event-ticketing-app/event-ticketing-backend.git
cd event-ticketing-backend
```

2. **Configure your environment**

Copy the example file and fill in your own values:

```
cp appsettings.example.json Access.API/appsettings.Development.json
```

3. **Apply migrations**

```
dotnet ef database update
```

4. **Run the API**

```
dotnet run
```

Open `https://localhost:<port>/swagger` to explore the endpoints.

---

## API Endpoints

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

### Users

| Method | Endpoint | Description | Auth |
|---|---|---|---|
| `GET` | `/api/users` | List all users | Yes |
| `GET` | `/api/users/{id}` | Get a user by ID | Yes |
| `PUT` | `/api/users/{id}` | Update a user | Yes |
| `DELETE` | `/api/users/{id}` | Delete a user | Yes |

### Tickets

| Method | Endpoint | Description | Auth |
|---|---|---|---|
| `POST` | `/api/tickets/reserve` | Reserve a ticket for an event | Yes |
| `POST` | `/api/tickets/purchase/{id}` | Confirm purchase of a reserved ticket | Yes |

---

## Roadmap

- [x] JWT Authentication
- [x] Events CRUD
- [x] Users CRUD
- [x] DTO pattern (Mass Assignment protection)
- [x] Service layer (TokenService, PasswordService, TicketService)
- [x] Password hashing (PasswordService)
- [x] User registration endpoint
- [x] Ticket model and reservation system (ReserveTicket)
- [ ] Ticket purchase flow (PurchaseTicket)
- [ ] Background job — auto-cancel expired reservations (Hangfire)
- [ ] Concurrency control (double-booking prevention)
- [ ] Role-based access control (Admin / User)
- [ ] Payment gateway (Stripe)
- [ ] QR code generation per ticket
- [ ] Ticket types (General, VIP)
- [ ] Repository layer
- [ ] Deployment (Azure / Railway)
- [ ] Cloud database

---

## Author

**Joel**
- GitHub: [@joeldc-dev](https://github.com/joeldc-dev)
- LinkedIn: [Joel Doña Corral](https://www.linkedin.com/in/joel-doña-corral-6667473b4/)
