# BookWorld API Documentation

     ## Overview
     BookWorld is a Content Management System (CMS) for managing a digital library of books, authors, and genres. This API allows administrators to perform CRUD operations and manage relationships (1-M: Author to Books, M-M: Books to Genres).

     ## Setup
     1. Install dependencies via NuGet:
        - `Microsoft.EntityFrameworkCore`
        - `Microsoft.EntityFrameworkCore.Tools`
        - `Microsoft.EntityFrameworkCore.SqlServer`
     2. Configure the connection string in `appsettings.json`:
        ```json
        "ConnectionStrings": {
          "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=BookWorld;Trusted_Connection=True;MultipleActiveResultSets=true"
        }