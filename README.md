# TCCD E-commerce API

A .NET 8 Web API project implementing an e-commerce system with clean architecture, featuring user authentication, product management, shopping cart functionality, and comprehensive logging.

## 🏗️ Architecture

This project follows **Clean Architecture** principles with the following layers:

- **Domain**: Core business entities and interfaces
- **Application**: Business logic, services, and DTOs
- **Infrastructure**: Data access, repositories, and external services
- **Presentation (TCCD-Task)**: Web API controllers and configuration

## 🚀 Technologies Used

- **.NET 8**
- **ASP.NET Core Web API**
- **Entity Framework Core** (SQL Server)
- **AutoMapper** for object mapping
- **Serilog** for structured logging
- **JWT Authentication**
- **Swagger/OpenAPI** for API documentation

## 📋 Prerequisites

Before running the project, ensure you have:

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (LocalDB or full instance)
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) or [Visual Studio Code](https://code.visualstudio.com/)

## ⚙️ Setup Instructions

### 1. Clone the Repository

### 2. Configure Database Connection
Update the connection string in `appsettings.json`:
{ "ConnectionStrings": { "DefaultConnection": "Server=(localdb)\mssqllocaldb;Database=EcommerceTCCD;Trusted_Connection=true;MultipleActiveResultSets=true" } }

### 3. Configure JWT Settings
Add JWT configuration in `appsettings.json`:
{ "Jwt": { "Key": "your-super-secret-key-here-minimum-32-characters", "Issuer": "TCCD-API", "Audience": "TCCD-Client" } }
### 4. Install Dependencies

### 5. Apply Database Migrations

### 6. Run the Application
The API will be available at:
- **HTTPS**: `https://localhost:7234`
- **HTTP**: `http://localhost:5287`
- **Swagger UI**: `https://localhost:7234/swagger`

## 📚 API Endpoints

### Authentication
- `POST /api/Auth/register` - Register new user
- `POST /api/Auth/login` - User login
- `POST /api/Auth/logout` - User logout

### Users
- `GET /api/User` - Get all users
- `GET /api/User/{id}` - Get user by ID
- `PUT /api/User/{id}` - Update user
- `DELETE /api/User/{id}` - Delete user

### Categories
- `GET /api/Category` - Get all categories
- `GET /api/Category/{id}` - Get category by ID
- `POST /api/Category` - Create category
- `PUT /api/Category/{id}` - Update category
- `DELETE /api/Category/{id}` - Delete category

### Products
- `GET /api/Product` - Get all products (with pagination)
- `GET /api/Product/{id}` - Get product by ID
- `POST /api/Product` - Create product
- `PUT /api/Product/{id}` - Update product
- `DELETE /api/Product/{id}` - Delete product

### Cart
- `POST /api/Cart` - Create cart
- `GET /api/Cart/cartItems/{cartId}` - Get cart items
- `DELETE /api/Cart/{id}` - Delete cart

### Cart Items
- `POST /api/CartItems/{cartId}` - Add item to cart
- `PUT /api/CartItems/{id}` - Update cart item
- `DELETE /api/CartItems/{id}` - Remove item from cart

## 🔐 Authentication

The API uses **JWT Bearer Token** authentication. To access protected endpoints:
1. Register a new user or login with existing credentials
2. Use the returned JWT token in the Authorization header: Authorization: Bearer <your-jwt-token>
