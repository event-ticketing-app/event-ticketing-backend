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

## 🛠️ Tech Stack

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
Accesso.API/
├── Controllers/
│   ├── AuthController.cs       # Registration & login endpoints
│   └── EventsController.cs     # Event CRUD endpoints
├── Data/
│   └── AppDbContext.cs          # EF Core database context
├── Models/
│   ├── Event.cs
│   ├── User.cs
│   ├── EventCreateDto.cs
│   ├── UserCreateDto.cs
│   └── LoginDto.cs
├── Services/
│   ├── ITokenService.cs         # Token service interface
│   └── TokenService.cs          # JWT generation logic
├── Migrations/                  # EF Core migration history
└── Program.cs                   # DI container & app configuration
```

---

##  Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- SQL Server (local or remote)
- A JWT secret key

### Setup

1. **Clone the repository**

```bash
git clone https://github.com/event-ticketing-app/event-ticketing-backend.git
cd event-ticketing-backend
```

2. **Configure your environment**

Update `appsettings.json` with your connection string and JWT key:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=TicketingDB;Trusted_Connection=True;"
},
"Jwt": {
  "Key": "YOUR_SECRET_KEY_HERE"
}
```

3. **Apply migrations**

```bash
dotnet ef database update
```

4. **Run the API**

```bash
dotnet run
```

Open `https://localhost:<port>/swagger` to explore the endpoints.

---

##  API Endpoints

### Auth

| Method | Endpoint | Description | Auth |
|---|---|---|---|
| `POST` | `/api/auth/register` | Register a new user | No |
| `POST` | `/api/auth/login` | Login and receive JWT | No |

### Events

| Method | Endpoint | Description | Auth |
|---|---|---|---|
| `GET` | `/api/eventos` | List all events | No |
| `POST` | `/api/eventos` | Create a new event | Yes |
| `PUT` | `/api/eventos/{id}` | Update an event | Yes |
| `DELETE` | `/api/eventos/{id}` | Delete an event | Yes |

---

##  Roadmap

- [x] JWT Authentication
- [x] Events CRUD
- [x] DTO pattern (Mass Assignment protection)
- [x] Service layer (TokenService)
- [ ] Users CRUD
- [ ] Repository layer
- [ ] Ticket reservation system
- [ ] Concurrency control (double-booking prevention)
- [ ] Role-based access control (Admin / User)

---

##  Author

**Joel**
- GitHub: [@TuUsuario](https://github.com/TuUsuario)
- LinkedIn: [Tu Perfil](https://linkedin.com/in/TuPerfil)
