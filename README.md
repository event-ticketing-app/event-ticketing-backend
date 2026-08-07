# event-ticketing-backend

> REST API for an event ticket sales platform — focused on real backend challenges like concurrency control, secure authentication, and clean architecture.

---

## About the Project

Built with ASP.NET Core (.NET 8) following a Controller-based architecture, with a dedicated Service layer for business logic.

This is not a basic CRUD. The project focuses on solving real-world backend problems:

- Preventing double-booking through optimistic concurrency control
- Protecting endpoints with JWT-based authentication
- Role-based access control (Admin, Organizer, User)
- Shielding database entities using the DTO pattern
- Keeping the codebase clean and maintainable as it scales

> **Active development** — features are being added progressively. See the [Roadmap](#roadmap) for current status.

---

## Features

- **JWT Authentication** — secure login and token generation via a dedicated `TokenService`
- **Role-Based Access Control** — three roles (`Admin`, `Organizer`, `User`) with endpoint-level protection via `[Authorize(Roles = "...")]`
- **Events API** — full CRUD for event management, restricted to Organizer and Admin roles
- **Users API** — full CRUD for user management, restricted to Admin role
- **Ticket Reservation System** — reserve and purchase flow with 10-minute expiration control
- **Background Job (Hangfire)** — automatically cancels expired reservations every 5 minutes, freeing up spots
- **Optimistic Concurrency Control** — prevents double-booking using EF Core RowVersion tokens, returns `409 Conflict` on collision
- **Global Error Handling** — `ExceptionMiddleware` catches all unhandled exceptions and returns clean JSON error responses
- **Service Layer** — `TokenService`, `PasswordService` and `TicketService` isolate business logic from controllers
- **Password Hashing** — secure registration using HMACSHA512 salt and hash via a dedicated `PasswordService`
- **DTO Pattern** — input and output DTOs prevent Mass Assignment attacks and decouple the API contract from the database schema
- **Repository Pattern** — `IEventRepository` / `EventRepository` decouples data access from business logic
- **Data Seeders** — Admin, Users (Organizers + User) and Events seeded automatically on startup
- **Organizer Ownership** — events are linked to their organizer via `OrganizerId`
- **EF Core Migrations** — database schema managed with Entity Framework Core
- **Swagger UI** — all endpoints documented and testable out of the box, with JWT Bearer authentication support

---

## Tech Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core (.NET 8) |
| Language | C# |
| ORM | Entity Framework Core 8 |
| Database | SQL Server |
| Auth | JWT (JSON Web Tokens) |
| Background Jobs | Hangfire |
| Docs | Swagger / Swashbuckle |

---

## Project Structure

```
Access.API/
├── Controllers/
│   ├── AuthController.cs        # Registration & login endpoints
│   ├── EventsController.cs      # Event CRUD endpoints
│   ├── TicketsController.cs     # Ticket reservation & purchase endpoints
│   └── UsersController.cs       # User management endpoints (Admin only)
├── Data/
│   └── AppDbContext.cs           # EF Core database context
├── DTOs/
│   ├── AdminUserCreateDto.cs
│   ├── EventCreateDto.cs
│   ├── EventResponseDto.cs
│   ├── UserCreateDto.cs
│   ├── UserResponseDto.cs
│   ├── UserUpdateDto.cs
│   ├── LoginDto.cs
│   ├── TicketReserveResponseDto.cs
│   └── TicketPurchaseResponseDto.cs
├── Enums/
│   ├── TicketStatus.cs           # Reserved, Purchased, Cancelled
│   └── UserRole.cs               # User, Organizer, Admin
├── Jobs/
│   └── ExpiredTicketsJob.cs      # Hangfire job to cancel expired reservations
├── Middleware/
│   └── ExceptionMiddleware.cs    # Global error handling
├── Models/
│   ├── Event.cs                  # Includes OrganizerId (FK to User)
│   ├── User.cs
│   └── Ticket.cs
├── Repositories/
│   ├── IEventRepository.cs       # Event repository interface
│   └── EventRepository.cs        # Event repository implementation
├── Seeders/
│   ├── AdminSeeder.cs            # Default Admin user seeder
│   ├── UsersSeeder.cs            # Organizer and User test accounts
│   └── EventSeeder.cs            # Sample events linked to organizers
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

### Environment Variables

| Variable | Description | Default (dev only) |
|---|---|---|
| `ADMIN_EMAIL` | Default admin user email | `admin@admin.com` |
| `ADMIN_PASSWORD` | Default admin user password | `123` |

> In production, set these in your hosting environment (e.g. Azure App Service → Configuration). In local development, the default values are used automatically if not set.

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

Open `http://localhost:<port>/swagger` to explore the endpoints.

> **On first run**, the app automatically seeds the database with:
> - An Admin user (credentials via environment variables)
> - Two Organizer accounts (`tenant1@gmail.com`, `tenant2@gmail.com`)
> - One User account (`user@gmail.com`)
> - Two sample events, one per organizer
>
> All test passwords: `123`

---

## API Endpoints

### Auth

| Method | Endpoint | Description | Auth |
|---|---|---|---|
| `POST` | `/api/auth/register` | Register a new user (role: User by default) | No |
| `POST` | `/api/auth/login` | Login and receive JWT | No |

### Events

| Method | Endpoint | Description | Auth |
|---|---|---|---|
| `GET` | `/api/events` | List all events | No |
| `POST` | `/api/events` | Create a new event | Organizer, Admin |
| `PUT` | `/api/events/{id}` | Update an event | Organizer, Admin |
| `DELETE` | `/api/events/{id}` | Delete an event | Organizer, Admin |

### Users

| Method | Endpoint | Description | Auth |
|---|---|---|---|
| `GET` | `/api/users` | List all users | Admin |
| `GET` | `/api/users/{id}` | Get a user by ID | Admin |
| `POST` | `/api/users` | Create a user with specific role | Admin |
| `PUT` | `/api/users/{id}` | Update a user | Admin |
| `DELETE` | `/api/users/{id}` | Delete a user | Admin |

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
- [x] Ticket purchase flow (PurchaseTicket)
- [x] Global error handling (ExceptionMiddleware)
- [x] Role-based access control (Admin / Organizer / User)
- [x] Background job — auto-cancel expired reservations (Hangfire)
- [x] Concurrency control — 409 Conflict on collision (RowVersion)
- [x] Repository Pattern (Events)
- [x] Admin Seeder with environment variable support
- [x] Data Seeders — Organizers, Users and sample Events on startup
- [x] OrganizerId linked to Event model
- [ ] Repository Pattern (Users, Tickets)
- [ ] Organizer ownership validation (only edit own events)
- [ ] Payment gateway (Stripe)
- [ ] QR code generation per ticket
- [ ] Ticket types (General, VIP)
- [ ] Deployment (Azure)
- [ ] Cloud database

---

## Author

**Joel**
- GitHub: [@joeldc-dev](https://github.com/joeldc-dev)
- LinkedIn: [Joel Doña Corral](https://www.linkedin.com/in/joeldona