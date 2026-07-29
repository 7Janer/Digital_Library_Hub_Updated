# Digital Library Hub

A .NET 8 ASP.NET Core MVC Library Management System with SQL Server LocalDB, Entity Framework Core, ASP.NET Core Identity, Bootstrap, search, pagination and role-based access.

## Fastest way to run

1. Install the **.NET 8 SDK** and **SQL Server Express LocalDB**.
2. Open the outer `DigitalLibraryHub.sln` file in Visual Studio 2022, or open the extracted folder in VS Code.
3. From the outer project folder run:

```powershell
dotnet restore
dotnet run --project LMSystem/LMSystem.csproj
```

4. Ctrl+click the localhost URL printed in the terminal.

The application automatically creates a fresh LocalDB database named `DigitalLibraryHub` and inserts sample records on first run. No migration command or SQL script is required.

## Demo accounts

| Role | Username | Password |
|---|---|---|
| Administrator | `admin@example.com` | `Password123` |
| Librarian | `librarian@example.com` | `Password123` |
| Member | `member@example.com` | `Password123` |
| Legacy administrator | `admin` | `12345` |

## Included modules

- Digital Library Hub dashboard and featured home page
- Book CRUD with details, validation, protected fields, search and pagination
- Borrow, return, due-date extension, overdue status and estimated fine
- Student CRUD with search and pagination
- Librarian CRUD with search and pagination
- Newspaper and magazine CRUD with search, type filtering and pagination
- ASP.NET Core Identity login, registration, logout, forgot/reset password, profile and change password
- Administrator user and role management
- About Us and functional Contact Us support tickets
- Custom not-found, unavailable, already-returned and access-denied pages
- xUnit, EF Core InMemory and FluentAssertions test project

## Run tests

From the outer project folder:

```powershell
dotnet test DigitalLibraryHub.sln
```

## Role access

- **Administrator:** complete access, including librarians, users/roles and support tickets.
- **Librarian:** catalog, publications, students and borrow/return operations.
- **Member:** dashboard, catalog, periodicals, profile and borrowing.

## Database note

The connection string uses:

```text
(localdb)\MSSQLLocalDB / DigitalLibraryHub
```

If LocalDB is missing, install SQL Server Express LocalDB and restart the terminal.
