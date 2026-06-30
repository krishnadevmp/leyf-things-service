# LeyfThings - Goal Tracker API

RESTful ASP.NET Core Web API powering the LeyfThings Goal Tracker application. The API provides goal and milestone management, AI-assisted goal generation, progress tracking, and persistent storage using Entity Framework Core and SQL Server.

---

## Features

- Goal CRUD APIs
- Milestone CRUD APIs
- Goal status updates
- Milestone status updates
- AI-powered goal generation
- Entity Framework Core
- SQL Server
- Swagger Documentation
- RESTful architecture
- Dependency Injection
- Repository/Service pattern

---

## Tech Stack

- ASP.NET Core
- Entity Framework Core
- SQL Server
- Swagger / OpenAPI
- OpenAI API
- C#

---

## Project Structure

```
LeyfThings
│
├── Controllers
│
├── Services
│
├── DTOs
│
├── Models
│
├── Data
│
├── Migrations
│
├── Interfaces
│
└── Program.cs
```

---

## API Modules

### Goals

```
GET      /api/goals

GET      /api/goals/{id}

POST     /api/goals

PUT      /api/goals/{id}

DELETE   /api/goals/{id}

PATCH    /api/goals/{id}/status
```

---

### Milestones

```
GET      /api/milestones

GET      /api/milestones/{id}

POST     /api/milestones

PUT      /api/milestones/{id}

DELETE   /api/milestones/{id}

PATCH    /api/milestones/{id}/status
```

---

### AI

```
POST /api/chat/create-goal
```

Generate an entire goal with milestones using natural language.

Example Request

```json
{
  "prompt": "I want to become a React expert in six months."
}
```

Response

```json
{
  "title": "...",
  "description": "...",
  "mileStones": [
    ...
  ]
}
```

---

## Running the Project

Clone repository

```bash
git clone https://github.com/yourusername/leyfthings-api.git

cd leyfthings-api
```

Restore packages

```bash
dotnet restore
```

Run migrations

```bash
dotnet ef database update
```

Run API

```bash
dotnet run
```

---

## Configuration

`appsettings.json`

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "..."
  },
  "OpenAI": {
    "ApiKey": "YOUR_API_KEY"
  }
}
```

For development, use User Secrets:

```bash
dotnet user-secrets init

dotnet user-secrets set "OpenAI:ApiKey" "YOUR_KEY"
```

---

## Swagger

Launch the application and navigate to

```
https://localhost:7223/swagger
```

---

## Architecture

```
Client
   │
React Application
   │
ASP.NET Core Controllers
   │
Service Layer
   │
Entity Framework Core
   │
SQL Server
```

---

## Roadmap

- AI Chat Assistant
- Semantic Search
- Goal Recommendations
- Reminder Service
- Email Notifications
- Authentication & Authorization
- User Profiles
- Goal Sharing
- Dashboard Analytics
- Background Jobs

---

## Frontend Repository

The frontend application is available in the corresponding React repository.
