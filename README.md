# SIG-T (Sistema de Gestión de Tareas)

A comprehensive full-stack task management system built with modern technologies following Excellentiam standards.

## Architecture Overview

The project follows a clean architecture with the following components:

```
SIG-T Solution
├── Domain (Shared Models & Entities)
├── SIG_T.Api (.NET 8 Minimal API)
├── SIG_T.Client (Blazor WebAssembly)
├── SIG_T.Worker (.NET Worker Service)
├── Database (SQL Server Scripts)
└── DeploymentDocs
```

## Technology Stack

- **Database**: SQL Server with Stored Procedures and Triggers
- **API**: .NET 8 Minimal API
- **Frontend**: Blazor WebAssembly (WASM)
- **Background Processing**: .NET Worker Service
- **Architecture**: Database-First approach

## Features

### Phase 1 - Database (Completed)
- ✅ SQL Server database schema
- ✅ Three main tables: Usuarios, Tareas, RegistroDeActividad
- ✅ Stored Procedures for CREATE and UPDATE operations
- ✅ Audit triggers for automatic activity logging
- ✅ Advanced views with INNER JOIN queries
- ✅ Performance indexes

### Phase 2 - API REST (In Progress)
- 🔄 .NET 8 Minimal API implementation
- 🔄 Async/await endpoints
- 🔄 DTOs with proper validation
- 🔄 Database-first approach with stored procedures
- 🔄 HTTP 202 Accepted for slow operations

### Phase 3 - Worker Service
- ⏳ Background task processing
- ⏳ Report generation simulation
- ⏳ Polling every 30 seconds

### Phase 4 - Blazor WebAssembly
- ⏳ Component-based UI
- ⏳ State management
- ⏳ Real-time updates

### Phase 5 - Deployment
- ⏳ IIS configuration
- ⏳ Environment-specific settings
- ⏳ SSL and security setup

## Database Schema

### Tables
- **Usuarios**: User management with active/inactive status
- **Tareas**: Task management with states (Pending, In Progress, Completed)
- **RegistroDeActividad**: Audit trail for all operations

### Stored Procedures
- `sp_Tareas_Create`: Creates new tasks with validation
- `sp_Tareas_Update`: Updates existing tasks with parameter binding

### Triggers
- `TR_Tareas_AfterInsert`: Auto-logs new task creation
- `TR_Tareas_AfterUpdate`: Auto-logs task updates
- `TR_Tareas_AfterDelete`: Auto-logs task deletion

### Views
- `VW_TareasConUsuario`: Tasks with complete user information
- `VW_UsuariosConCantidadTareas`: Users with task statistics
- `fn_GetTaskStatisticsByUser`: Function for user-specific statistics

## Getting Started

### Prerequisites
- .NET 8 SDK
- SQL Server
- Visual Studio 2022 or VS Code

### Database Setup
1. Execute the SQL scripts in order:
   - `01_CreateDatabase.sql`
   - `02_CreateTables.sql`
   - `03_CreateStoredProcedures.sql`
   - `04_CreateTrigger.sql`
   - `05_CreateViews.sql`
   - `06_SampleData.sql`

### Running the Application
1. Configure connection string in `appsettings.json`
2. Run the API project
3. Run the Blazor client
4. Access the application in your browser

## API Endpoints

### Tasks Management
- `GET /api/tareas` - Get all tasks
- `GET /api/tareas/{id}` - Get task by ID
- `POST /api/tareas` - Create new task (uses stored procedure)
- `PUT /api/tareas/{id}` - Update task (uses stored procedure)
- `DELETE /api/tareas/{id}` - Delete task
- `POST /api/reporte/tareas-finalizadas` - Enqueue a request to generate a report of completed tasks (HTTP 202); Worker polls queue and processes reports.

### Users Management
- `GET /api/usuarios` - Get all users
- `GET /api/usuarios/{id}` - Get user by ID
- `POST /api/usuarios` - Create new user
- `PUT /api/usuarios/{id}` - Update user
- `DELETE /api/usuarios/{id}` - Delete user

## Security Features
- SQL Injection prevention through parameterized stored procedures
- Input validation and sanitization
- Error handling with proper HTTP status codes
- CORS configuration for cross-origin requests

## Monitoring and Logging
- Automatic activity logging through database triggers
- Comprehensive error handling and logging
- Performance monitoring through database indexes

## Development Guidelines
- Follow database-first approach for critical operations
- Use async/await for all database operations
- Implement proper DTOs for API contracts
- Maintain clean separation of concerns
- Follow SOLID principles

## Deployment
See `DeploymentInstructions.txt` for IIS deployment configuration.

---
**Developed for Excellentiam Induction Process**
**Date**: 2025-12-17
**Version**: 1.0.0