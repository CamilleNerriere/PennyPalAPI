CREATE DATABASE PennyPal;
GO

USE PennyPal;
GO

CREATE TABLE users (
    id INT IDENTITY(1,1) PRIMARY KEY
    , lastname NVARCHAR(50) NOT NULL
    , firstname NVARCHAR(50) NOT NULL
    , email NVARCHAR(100) UNIQUE NOT NULL 
)
GO

CREATE TABLE auth (
    email NVARCHAR(100) PRIMARY KEY NOT NULL
    , password_hash VARBINARY(MAX) NOT NULL
    , password_salt VARBINARY(MAX) NOT NULL
    , role NVARCHAR(50) DEFAULT 'user'
    , CONSTRAINT FK_Auth_Users FOREIGN KEY (email)
    REFERENCES users(email)
    ON DELETE CASCADE
)
GO

CREATE TABLE expense_categories (
    id INT IDENTITY(1,1) PRIMARY KEY
    , user_id INT NOT NULL 
    , name NVARCHAR(100) NOT NULL
    , monthly_budget DECIMAL(10, 2) NOT NULL
    CONSTRAINT FK_ExpenseCategories_Users FOREIGN KEY (user_id)
    REFERENCES users(id)
    ON DELETE CASCADE
)
GO

CREATE NONCLUSTERED INDEX IX_ExpenseCategories_UserID
ON expense_categories (user_id)
GO

CREATE TABLE expenses (
    id INT IDENTITY(1,1) PRIMARY KEY
    , user_id INT NOT NULL
    , category_id INT NOT NULL
    , amount DECIMAL(10,2) NOT NULL
    , date DATETIME DEFAULT GETDATE()
    , CONSTRAINT FK_Expenses_Users FOREIGN KEY (user_id)
    REFERENCES users(id)
    ON DELETE CASCADE
    , CONSTRAINT FK_Expenses_Expense_Categories FOREIGN KEY (category_id)
    REFERENCES expense_categories(id)
    ON DELETE CASCADE
)
GO

CREATE NONCLUSTERED INDEX IX_Expenses_UserID
ON expenses (user_id)
GO

CREATE NONCLUSTERED INDEX IX_Expenses_CategoryID
ON expenses (category_id)
GO