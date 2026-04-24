Library System REST API
This project is an ASP.NET Core Web API for managing a simple library system. It includes endpoints for books, members, and borrowing operations, along with Swagger support and API key authentication.

Overview
The API is structured to model the core parts of a basic library workflow:

managing books
managing members
tracking borrowing activity
protecting endpoints with API key authentication
The project uses an in-memory database, which makes it easy to run locally for learning, demos, and testing.

Tech Stack
ASP.NET Core Web API (.NET 8)
Entity Framework Core
InMemory database provider
Swagger / OpenAPI
Main API Areas
From the current controller structure, the API includes:

BooksController
MembersController
BorrowingController
The codebase also includes:

custom API key authentication
service abstractions
library domain models
Project Structure
LibrarySystem/Controllers/ for API endpoints
LibrarySystem/Models/ for domain models such as books and members
LibrarySystem/Data/ for the database context
LibrarySystem/Services/ for supporting services
LibrarySystem/Authentication/ for API key authentication logic
Getting Started
Prerequisites
.NET 8 SDK
Run the API
Clone the repository.
Open the solution:
