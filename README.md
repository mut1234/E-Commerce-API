📋 Project Overview
Simple E-Commerce API built with ASP.NET Core featuring user authentication, product management, and invoice generation.
🚀 Features

✅ JWT Authentication
✅ User Registration & Login
✅ Role-based Authorization (Admin/Visitor)
✅ Product CRUD with Soft Delete
✅ Paginated Product Listing
✅ Invoice Generation
✅ Sql Server Database with Seeded Data

🛠️ Technologies

ASP.NET Core 8.0
Entity Framework Core (Sql Server)
JWT Authentication
BCrypt for Password Hashing
Swagger/OpenAPI

📦 Installation & Setup
Prerequisites

.NET 8.0 SDK or later
Any IDE (Visual Studio, VS Code, Rider)

Steps to Run

Clone the repository

bashgit clone <your-repo-url>
cd ECommerceAPI

Restore packages

bashdotnet restore

Run the application

bashdotnet run

Access Swagger UI
Open browser and navigate to: https://localhost:7138 

🔐 Default Credentials
Admin Account
Username: Admin
Password: Admin@123

Regular Vistor Account

Username: Vistor
Password: Vistor@123

project can be improved :
add service 
clean architecture 
repository patern + unit of work
meditor
auto mapper
