# PennyPal API

PennyPal is a simple budget management API built with C# and .NET. It allows users to track their expenses, manage categories, and analyze spending trends. The API uses JWT authentication and is backed by SQL Server.

## Features

- User authentication (register, login, password change, account deletion)
- Expense tracking with category management
- Spending trend analysis
- Secure endpoints with JWT authentication
- Built with .NET 8 and SQL Server

## Technologies Used

- **Backend:** C# (.NET 8, ASP.NET Core)
- **Database:** SQL Server
- **Authentication:** JWT

## Installation

1. Clone the repository:
   ```sh
   git clone https://github.com/yourusername/PennyPal.git
   cd PennyPal
   ```
2. Set up your SQL Server database and update the connection string in `appsettings.json`.
3. Run database migrations:
   ```sh
   dotnet ef database update
   ```
4. Start the application:
   ```sh
   dotnet run
   ```

## API Endpoints

### Authentication
- `POST /Auth/Register` - Register a new user
- `POST /Auth/Login` - Log in and receive a JWT
- `PUT /Auth/ChangePassword` - Change user password (requires authentication)
- `DELETE /Auth` - Delete user account (requires authentication)

### Users
- `GET /User/Users` - Get all users (requires authentication)
- `GET /User/{id}` - Get user by ID (requires authentication)
- `PUT /User` - Update user information (requires authentication)

### Expense Categories
- `GET /ExpenseCategory` - Get all categories (requires authentication)
- `GET /ExpenseCategory/{id}` - Get category by ID (requires authentication)
- `POST /ExpenseCategory` - Add a category (requires authentication)
- `PUT /ExpenseCategory` - Update a category (requires authentication)
- `DELETE /ExpenseCategory/{id}` - Delete a category (requires authentication)

### Expenses
- `GET /Expense` - Get expenses with filters (requires authentication)
- `GET /Expense/Tendances` - Get expense trends (requires authentication)
- `POST /Expense/Add` - Add an expense (requires authentication)
- `PUT /Expense/Update` - Update an expense (requires authentication)
- `DELETE /Expense/Delete/{id}` - Delete an expense (requires authentication)


## License
This project is open-source and available under the MIT License.

