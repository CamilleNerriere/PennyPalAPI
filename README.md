# PennyPal API

PennyPal is a simple budget management API built with C# and .NET. It allows users to track their expenses, manage categories, and analyze spending trends. The API uses JWT authentication, with secure token rotation managed via cookies and is backed by SQL Server.

## Features

- User authentication (register, login, password change, account deletion)
- Expense tracking with category management
- Spending trend analysis
- Secure endpoints with JWT authentication
- Built with .NET 8 and SQL Server

## Technologies Used

- **Backend:** C# (.NET 8, ASP.NET Core)
- **Database:** SQL Server
- **Authentication:** JWT with secure token rotation managed via cookies

## Usage

PennyPal is not a fully standalone backend out of the box. To use it properly, you have two options:

### Option 1 – Manual setup (advanced)

You can use the backend separately, but you will need to:

    Create your own SQL Server database

    Configure the connection string in appsettings.Development.json

    Run the SQL scripts provided in the scripts/ folder to create tables and seed initial data

### Option 2 – Docker setup (recommended)

The easiest way to run the full application (frontend + backend + database) is via Docker. You can find the repository [here](https://github.com/CamilleNerriere/PennyPal)

## Installation (manual setup)

1. Clone the repository:
   ```sh
   git clone git@github.com:CamilleNerriere/PennyPalAPI.git
   cd PennyPal
   ```
2. Set up your database 

   You will find scripts to create the database, tables and seeds in the folder "Scripts"

3. Set up the appsettings.json

   ``` 
   cp appsettings.Development.example.json appsettings.Development.json 
   ```

   Then fill in the required values:

   * DefaultConnection: your SQL Server connection string

   * PasswordKey and TokenKey: secret keys used for password hashing and JWT signing

   * React_URL: the frontend app URL (e.g., http://localhost:5173)

3. Start the application:
   ```sh
   dotnet run
   ```

## API Endpoints

### Authentication
- `POST /Auth/Register` - Register a new user
- `POST /Auth/Login` - Log in and receive a JWT access token in the response body, along with a refresh token automatically stored in a secure HTTP-only cookie for token rotation.
- `POST /Auth/RefreshToken`- Check cookie validity, revoke old token and receive a new JWT access token and updated cookie. 
- `PUT /Auth/ChangePassword` - Change user password (requires authentication)
- `PUT /Auth/Logout`- Revoke token
- `DELETE /Auth` - Delete user account (requires authentication)

### Users
- `GET /User/` - Retrieves the authenticated user’s information. The user ID is inferred from the JWT. (requires authentication)
- `PUT /User` - Update user information (requires authentication)
- `GET /User/UserRemain` - Returns the authenticated user’s remaining budget. (requires authentication)
- `GET /User/UserCategories/Remain`- Returns the authenticated user’s remaining budget per category. (requires authentication)

### Expense Categories
- `GET /ExpenseCategory` - Get all categories (requires authentication)
- `GET /ExpenseCategory/{id}` - Get category by ID (requires authentication)
- `GET /ExpenseCategory/MonthlyBudget`- Get user's monthly budget by category (requires authentication)
- `POST /ExpenseCategory` - Add a category (requires authentication)
- `PUT /ExpenseCategory` - Update a category (requires authentication)
- `DELETE /ExpenseCategory/{id}` - Delete a category (requires authentication)

### Expenses
- `GET /Expense/{id}`- Get expenses data by ID (requires authentication)
- `GET /Expense` - Get expenses with filters (requires authentication)
- `GET /Expense/Tendances` - Get expense trends (requires authentication)
- `POST /Expense/Add` - Add an expense (requires authentication)
- `PUT /Expense/Update` - Update an expense (requires authentication)
- `DELETE /Expense/Delete/{id}` - Delete an expense (requires authentication)


## License
This project is open-source and available under the MIT License.

