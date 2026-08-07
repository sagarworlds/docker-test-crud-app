<div align="center">

# 📦 CrudApp

### A modern full-stack CRUD application for managing a product catalog

[![Angular](https://img.shields.io/badge/Angular-22-DD0031?style=for-the-badge&logo=angular&logoColor=white)](https://angular.dev)
[![.NET](https://img.shields.io/badge/.NET-10-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-16-336791?style=for-the-badge&logo=postgresql&logoColor=white)](https://www.postgresql.org)
[![Docker](https://img.shields.io/badge/Docker-Compose-2496ED?style=for-the-badge&logo=docker&logoColor=white)](https://www.docker.com)

</div>

---

## 📋 Table of Contents

- [Overview](#-overview)
- [Tech Stack](#-tech-stack)
- [Architecture](#-architecture)
- [Project Structure](#-project-structure)
- [Getting Started](#-getting-started)
- [API Reference](#-api-reference)
- [Angular 22 Features](#-angular-22-features)
- [Environment Variables](#-environment-variables)
- [Deploying to Render](#-deploying-to-render)
- [Useful Commands](#-useful-commands)

---

## 🌟 Overview

CrudApp is a **production-ready**, fully containerized full-stack web application that demonstrates a clean CRUD implementation using modern technologies.

**Features:**
- ✅ Create, Read, Update, Delete products
- ✅ Live search / filter by name, category, or description
- ✅ Form validation with inline error messages
- ✅ Delete confirmation dialog
- ✅ Loading skeletons & empty states
- ✅ Responsive dark-mode UI
- ✅ Interactive API docs (Scalar UI)
- ✅ One-command local setup with Docker Compose
- ✅ One-click cloud deploy with Render Blueprints

---

## 🛠 Tech Stack

| Layer       | Technology                                          |
|-------------|-----------------------------------------------------|
| **Frontend**| Angular 22 · Signals · OnPush Change Detection      |
| **Backend** | .NET 10 Web API · Entity Framework Core · Npgsql    |
| **Database**| PostgreSQL 16                                       |
| **API Docs**| Scalar UI (OpenAPI 3.0)                             |
| **DevOps**  | Docker · Docker Compose · Render.com                |

---

## 🏗 Architecture

```
┌─────────────────────────────────────────────────┐
│                    Browser                       │
└───────────────────────┬─────────────────────────┘
                        │
                        ▼
┌─────────────────────────────────────────────────┐
│          Angular 22  (Nginx — Port 80)           │
│   Standalone Components · Signals · OnPush CD    │
│              http://localhost:4200               │
└───────────────────────┬─────────────────────────┘
                        │  Proxies /api/* requests
                        ▼
┌─────────────────────────────────────────────────┐
│         .NET 10 Web API  (Port 8080)             │
│          EF Core · Npgsql · Scalar UI            │
│              http://localhost:8080               │
└───────────────────────┬─────────────────────────┘
                        │  TCP 5432
                        ▼
┌─────────────────────────────────────────────────┐
│          PostgreSQL 16  (Port 5432)              │
│             Persistent Docker Volume             │
└─────────────────────────────────────────────────┘
```

---

## 📁 Project Structure

```
Docker-Test-CRUD-App/
│
├── 📄 docker-compose.yml          # Orchestrates all 3 containers
├── 📄 render.yaml                 # Render.com deploy config (IaC)
├── 📄 .env                        # Local secrets  ← gitignored
├── 📄 .env.example                # Template for .env
├── 📄 .gitignore
├── 📄 README.md
│
├── 📂 api/                        # .NET 10 Web API
│   ├── 📄 Dockerfile
│   ├── 📄 CrudApp.Api.csproj
│   ├── 📄 Program.cs              # Bootstrap · EF init · seed data
│   ├── 📄 appsettings.json
│   ├── 📂 Controllers/
│   │   └── 📄 ProductsController.cs
│   ├── 📂 Data/
│   │   └── 📄 AppDbContext.cs
│   ├── 📂 DTOs/
│   │   └── 📄 ProductDto.cs
│   └── 📂 Models/
│       └── 📄 Product.cs
│
└── 📂 frontend/                   # Angular 22
    ├── 📄 Dockerfile
    ├── 📄 nginx.conf              # SPA routing + /api proxy
    ├── 📄 angular.json
    ├── 📄 package.json
    ├── 📄 tsconfig.json
    └── 📂 src/
        ├── 📄 styles.css          # Global dark-mode design system
        └── 📂 app/
            ├── 📄 app.component.{ts,html,css}
            ├── 📄 app.config.ts
            ├── 📄 app.routes.ts
            ├── 📂 models/
            │   └── 📄 product.model.ts
            ├── 📂 services/
            │   └── 📄 product.service.ts
            └── 📂 components/
                ├── 📂 product-list/    ← .ts · .html · .css
                ├── 📂 product-form/    ← .ts · .html · .css
                └── 📂 confirm-dialog/  ← .ts · .html · .css
```

---

## 🚀 Getting Started

### Prerequisites

- [Docker Desktop](https://www.docker.com/products/docker-desktop/) installed and running
- Git

### 1 — Clone the repository

```bash
git clone https://github.com/YOUR-USERNAME/YOUR-REPO.git
cd YOUR-REPO
```

### 2 — Create your `.env` file

```bash
cp .env.example .env
```

> The default values in `.env.example` work out of the box for local development.

### 3 — Start all containers

```bash
docker-compose up --build
```

Docker will automatically:
1. 🐘 Start PostgreSQL and wait until it's healthy
2. ⚙️ Build the .NET 10 API, create the DB schema, and seed 5 sample products
3. 🅰️ Build Angular 22 and serve it via Nginx

### 4 — Open the app

| Service             | URL                                     |
|---------------------|-----------------------------------------|
| 🅰️ Angular UI       | http://localhost:4200                   |
| 📖 API Scalar Docs  | http://localhost:8080/scalar/v1         |
| 🔌 OpenAPI JSON     | http://localhost:8080/openapi/v1.json   |
| 🐘 PostgreSQL       | `localhost:5432` / db: `CrudAppDb`      |

---

## 📡 API Reference

**Base URL:** `http://localhost:8080/api`

| Method     | Endpoint              | Description                        |
|------------|-----------------------|------------------------------------|
| `GET`      | `/products`           | Get all products                   |
| `GET`      | `/products?search=x`  | Search by name, category or desc   |
| `GET`      | `/products/{id}`      | Get a single product by ID         |
| `POST`     | `/products`           | Create a new product               |
| `PUT`      | `/products/{id}`      | Update an existing product         |
| `DELETE`   | `/products/{id}`      | Delete a product                   |

### Request Body — `POST` / `PUT`

```json
{
  "name": "Wireless Headphones",
  "description": "Noise-cancelling over-ear headphones",
  "price": 299.99,
  "category": "Electronics"
}
```

### Response — Product object

```json
{
  "id": 1,
  "name": "Wireless Headphones",
  "description": "Noise-cancelling over-ear headphones",
  "price": 299.99,
  "category": "Electronics",
  "createdAt": "2026-08-07T06:51:00Z",
  "updatedAt": null
}
```

---

## ✨ Angular 22 Features

| Feature | Used In |
|---------|---------|
| `signal()` & `computed()` | Product list — reactive state & filtered results |
| `input()` & `output()` signals | `ConfirmDialogComponent` — signal-based I/O |
| `ChangeDetectionStrategy.OnPush` | All components (default in Angular 22) |
| `@if` / `@for` control flow | All HTML templates |
| `loadComponent()` lazy routes | `app.routes.ts` |
| Standalone components | All components — no NgModules |
| Reactive Forms | `ProductFormComponent` |
| `withViewTransitions()` | Smooth page transitions |

---

## 🔐 Environment Variables

### `.env` — Local Development

| Variable            | Default Value      | Description             |
|---------------------|--------------------|-------------------------|
| `POSTGRES_USER`     | `postgres`         | PostgreSQL username      |
| `POSTGRES_PASSWORD` | `Str0ng!P@ssw0rd`  | PostgreSQL password      |
| `POSTGRES_DB`       | `CrudAppDb`        | Database name            |

> ⚠️ **Never commit `.env` to Git.** It is listed in `.gitignore`. Use `.env.example` as a safe, committable template.

---

## ☁️ Deploying to Render

Everything is defined in [`render.yaml`](./render.yaml) — deploy all 3 services in one click.

### Services on Render

| Service         | Type         | Cost                       |
|-----------------|--------------|----------------------------|
| `crud-frontend` | Static Site  | **Free — forever**         |
| `crud-api`      | Web Service  | Free (cold starts) / $7/mo |
| `crud-db`       | PostgreSQL   | Free 90 days / $7/mo       |

### Deployment Steps

**Step 1** — Push to GitHub

```bash
git add .
git commit -m "Initial commit"
git remote add origin https://github.com/YOUR-USERNAME/YOUR-REPO.git
git push -u origin main
```

**Step 2** — Deploy via Blueprint

1. Go to [render.com](https://render.com) → **New → Blueprint**
2. Connect your GitHub repo
3. Render detects `render.yaml` and shows a preview of all 3 services:
   - `crud-db` — PostgreSQL (free)
   - `crud-api` — Web Service (Docker)
   - `crud-frontend` — Static Site (free)
4. Click **Apply** — done!

> The database connection string is **auto-injected** into the API. No manual copy-paste.

**Step 3** — Set the production API URL

Once the API is live, copy its URL (e.g. `https://crud-api-xxxx.onrender.com`) and update:

```typescript
// frontend/src/environments/environment.prod.ts
export const environment = {
  production: true,
  apiUrl: 'https://crud-api-xxxx.onrender.com'  // ← paste here
};
```

```bash
git add .
git commit -m "Set production API URL"
git push   # Render auto-redeploys the frontend
```

---

## 💻 Useful Commands

```bash
# Start all containers (first time or after changes)
docker-compose up --build

# Start containers in background
docker-compose up -d

# View live logs
docker-compose logs -f

# View logs for a specific service
docker-compose logs -f api
docker-compose logs -f frontend
docker-compose logs -f postgres

# Stop all containers
docker-compose down

# Stop and DELETE the database volume (full reset)
docker-compose down -v

# Rebuild a single service
docker-compose up --build api
```

---

## 📄 License

This project is licensed under the **MIT License** — free to use, modify, and distribute.
