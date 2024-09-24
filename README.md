# Task Management API

## Overview

A RESTful API built using ASP.NET Core (.NET 8) for managing tasks, users, and projects. This API allows for the creation, updating, assignment, and tracking of tasks across multiple projects, along with user management.

---

## Features

- **User Management:** Create, update, delete, and retrieve user data.
- **Task Management:** Create, update, delete, and retrieve tasks associated with users and projects.
- **Project Management:** Manage projects and associate users and tasks with each project.
- **Swagger Integration:** Comprehensive API documentation with descriptions for each endpoint.
- **NUnit Testing:** Unit tests for services and repositories.

---

## Technologies

- **ASP.NET Core** (.NET 8)
- **Entity Framework Core**
- **NUnit** (for unit testing)
- **Swagger / Swashbuckle**
- **SQL Server**

---

## Getting Started

### Prerequisites

Ensure you have the following installed:

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- Visual Studio or Visual Studio Code

### Setup Instructions

1. **Clone the repository:**

    ```bash
    git clone https://github.com/marijatopalova/task-management-api.git
    cd task-management-api
    ```

2. **Update the connection string:**

    In `appsettings.json`, update the connection string for your SQL Server instance:

    ```json
    "ConnectionStrings": {
        "DefaultConnection": "Server=YOUR_SERVER;Database=TaskManagementDb;Trusted_Connection=True;MultipleActiveResultSets=true"
    }
    ```

3. **Run database migrations:**

    Apply the migrations to your database:

    ```bash
    dotnet ef database update
    ```

4. **Run the application:**

    Start the application:

    ```bash
    dotnet run
    ```

5. **Access Swagger UI:**

    Open [http://localhost:44330/swagger](http://localhost:44330/swagger) to explore the API documentation.

---

## API Endpoints

### User Endpoints

- **GET** `/api/user` - Get a list of users
- **POST** `/api/user` - Create a new user
- **GET** `/api/user/{userId}` - Get a user by ID
- **GET** `/api/user/project/{projectId}` - Get a list of users by project ID

### Task Endpoints

- **GET** `/api/task/user/{userId}` - Get a task by user ID
- **POST** `/api/task` - Create a new task
- **GET** `/api/task/project/{projectId}` - Get a task by project ID
- **PATCH** `/api/task/{taskId}` - Update a task
- **GET** `/api/task/{taskId}` - Get a task by ID

### Project Endpoints

- **GET** `/api/project` - Get a list of projects
- **POST** `/api/project` - Create a new project
- **GET** `/api/project/{projectId}` - Get a project by ID
- **POST** `/api/project/{projectId}/user/{userId}` - Add user to a project
- **DELETE** `/api/project/{projectId}/user/{userId}` - Delete a user from the project

---

## Testing

Unit tests are implemented using **NUnit**. To run the tests:

```bash
dotnet test
