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
- **Admin API** — separate controllers per resource (`AdminUsersController`, `AdminEventsController`, `AdminTicketsController`) plus dashboard metrics
- **Ticket Reservation System** — reserve and purchase flow with 10-minute expiration control
- **Ticket Cancellation** — users can manually cancel a reserved ticket before expiry
- **Background Job (Hangfire)** — automatically cancels expired reservations every 5 minutes, freeing up spots
- **Optimistic Concurrency Control** — prevents double-booking using EF Core RowVersion tokens, returns `409 Conflict` on collision
- **Global Error Handling** — `ExceptionMiddleware` catches all unhandled exceptions and returns clean JSON error responses
- **Service Layer** — `TokenService`, `PasswordService` and `TicketService` isolate business logic from controllers
- **Password Hashing** — secure registration using HMACSHA512 per-user salt and hash via a dedicated `PasswordService`
- **DTO Pattern** — separate DTOs per consumer (public vs admin) prevent Mass Assignment attacks and decouple the API contract from the database schema
- **Repository Pattern** — `IEventRepository`, `ITicketRepository` and `IUserRepository` decouple data access from business logic
- **Data Seeders** — Admin, Users (Organizers + User) and Events seeded automatically on startup
- **Organizer Ownership** — events are linked to their organizer via `OrganizerId`; Organizers can only edit/delete their own events
- **Input Validation** — Data Annotations on DTOs prevent negative prices and capacities
- **Last Admin Protection** — the API prevents deleting the last Admin user (returns `400 Bad Request`)
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
│   ├── AuthController.cs            # Registration & login endpoints
│   ├── EventsController.cs          # Event CRUD endpoints (Organizer/Admin)
│   ├── TicketsController.cs         # Ticket reservation, purchase, cancellation
│   ├── AdminController.cs           # Dashboard metrics
│   ├── AdminUsersController.cs      # Admin user management
│   ├── AdminEventsController.cs     # Admin event management
│   └── AdminTicketsController.cs    # Admin ticket management
├── Data/
│   └── AppDbContext.cs
├── DTOs/
│   ├── AdminEventCreateDto.cs
│   ├── AdminEventResponseDto.cs
│   ├── AdminTicketResponseDto.cs
│   ├── AdminUserCreateDto.cs
│   ├── EventCreateDto.cs
│   ├── EventResponseDto.cs
│   ├── EventUpdateDto.cs
│   ├── UserCreateDto.cs
│   ├── UserResponseDto.cs
│   ├── UserUpdateDto.cs
│   ├── LoginDto.cs
│   ├── TicketReserveDto.cs
│   ├── TicketReserveResponseDto.cs
│   └── TicketPurchaseResponseDto.cs
├── Enums/
│   ├── TicketStatus.cs              # Reserved, Purchased, Cancelled
│   └── UserRole.cs                  # User, Organizer, Admin
├── Jobs/
│   └── ExpiredTicketsJob.cs
├── Middleware/
│   └── ExceptionMiddleware.cs
├── Models/
│   ├── Event.cs
│   ├── User.cs
│   └── Ticket.cs
├── Repositories/
│   ├── IEventRepository.cs
│   ├── EventRepository.cs
│   ├── ITicketRepository.cs
│   ├── TicketRepository.cs
│   ├── IUserRepository.cs
│   └── UserRepository.cs
├── Seeders/
│   ├── AdminSeeder.cs
│   ├── UsersSeeder.cs
│   └── EventSeeder.cs
├── Services/
│   ├── ITokenService.cs / TokenService.cs
│   ├── IPasswordService.cs / PasswordService.cs
│   └── ITicketService.cs / TicketService.cs
├── Migrations/
└── Program.cs
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

### Setup

1. **Clone the repository**

```
git clone https://github.com/event-ticketing-app/event-ticketing-backend.git
cd event-ticketing-backend
```

2. **Configure your environment**

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

> **On first run**, the app seeds the database with an Admin user, two Organizer accounts, one User account and two sample events. All test passwords: `123`

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
| `GET` | `/api/events/{id}` | Get event by ID | No |
| `GET` | `/api/events/my-events` | Get organizer's own events | Organizer |
| `POST` | `/api/events` | Create a new event | Organizer, Admin |
| `PUT` | `/api/events/{id}` | Update an event | Organizer, Admin |
| `DELETE` | `/api/events/{id}` | Delete an event | Organizer, Admin |

### Tickets

| Method | Endpoint | Description | Auth |
|---|---|---|---|
| `POST` | `/api/tickets/reserve` | Reserve a ticket | Yes |
| `POST` | `/api/tickets/purchase/{id}` | Confirm purchase | Yes |
| `POST` | `/api/tickets/cancel/{id}` | Cancel a reservation | Yes |
| `GET` | `/api/tickets/{id}` | Get ticket details | Yes |
| `GET` | `/api/tickets/my-tickets` | Get user's tickets | Yes |

### Admin

| Method | Endpoint | Description | Auth |
|---|---|---|---|
| `GET` | `/api/admin/dashboard` | Platform metrics | Admin |
| `GET` | `/api/admin/users` | List all users | Admin |
| `GET` | `/api/admin/users/{id}` | Get user by ID | Admin |
| `POST` | `/api/admin/users` | Create a user | Admin |
| `PUT` | `/api/admin/users/{id}` | Update a user | Admin |
| `DELETE` | `/api/admin/users/{id}` | Delete a user (last admin protected) | Admin |
| `GET` | `/api/admin/users/organizers` | List organizers | Admin |
| `GET` | `/api/admin/events` | List all events | Admin |
| `GET` | `/api/admin/events/{id}` | Get event by ID | Admin |
| `POST` | `/api/admin/events` | Create an event | Admin |
| `PUT` | `/api/admin/events/{id}` | Update an event | Admin |
| `DELETE` | `/api/admin/events/{id}` | Delete an event | Admin |
| `GET` | `/api/admin/tickets` | List all tickets | Admin |
| `GET` | `/api/admin/tickets/{id}` | Get ticket by ID | Admin |
| `PUT` | `/api/admin/tickets/{id}/cancel` | Cancel a ticket | Admin |
| `DELETE` | `/api/admin/tickets/{id}` | Delete a ticket | Admin |

---

## Roadmap

- [x] JWT Authentication
- [x] Events CRUD with organizer ownership validation
- [x] DTO pattern — separate DTOs per consumer (public vs admin)
- [x] Service layer (TokenService, PasswordService, TicketService)
- [x] Password hashing — HMACSHA512 with per-user salt
- [x] Ticket reservation system with 10-minute expiry
- [x] Ticket purchase flow
- [x] Ticket cancellation (manual)
- [x] My tickets endpoint
- [x] Global error handling (ExceptionMiddleware)
- [x] Role-based access control (Admin / Organizer / User)
- [x] Background job — auto-cancel expired reservations (Hangfire)
- [x] Concurrency control — 409 Conflict on collision (RowVersion)
- [x] Repository Pattern (Events, Tickets, Users)
- [x] Data Seeders — Organizers, Users and sample Events on startup
- [x] Admin panel — separate controllers per resource
- [x] Admin tickets CRUD (list, cancel, delete)
- [x] Input validation — no negative prices or capacities
- [x] Last Admin protection
- [ ] Pepper — add server-side secret to password hashing
- [ ] Organizer dashboard — per-event metrics
- [ ] Payment gateway (Stripe)
- [ ] QR code generation per ticket
- [ ] Ticket types (General, VIP)
- [ ] Logging
- [ ] Unit tests
- [ ] Azure DevOps CI/CD
- [ ] Deployment (Azure)
- [ ] Cloud database

---

## Author

**Joel**
- GitHub: [@joeldc-dev](https://github.com/joeldc-dev)
- LinkedIn: [Joel Doña Corral](https://www.linkedin.com/in/joeldona/)