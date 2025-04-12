USE PennyPal;

IF OBJECT_ID('refresh_tokens', 'U') IS NOT NULL DROP TABLE refresh_tokens;
IF OBJECT_ID('expenses', 'U') IS NOT NULL DROP TABLE expenses;
IF OBJECT_ID('expense_categories', 'U') IS NOT NULL DROP TABLE expense_categories;
IF OBJECT_ID('auth', 'U') IS NOT NULL DROP TABLE auth;
IF OBJECT_ID('users', 'U') IS NOT NULL DROP TABLE users;

CREATE TABLE users (
    id INT IDENTITY(1,1) PRIMARY KEY,
    lastname NVARCHAR(50) NOT NULL,
    firstname NVARCHAR(50) NOT NULL,
    email NVARCHAR(100) UNIQUE NOT NULL 
);

CREATE TABLE auth (
    email NVARCHAR(100) PRIMARY KEY NOT NULL,
    password_hash VARBINARY(MAX) NOT NULL,
    password_salt VARBINARY(MAX) NOT NULL,
    CONSTRAINT FK_Auth_Users FOREIGN KEY (email)
        REFERENCES users(email)
        ON DELETE CASCADE
);


CREATE TABLE expense_categories (
    id INT IDENTITY(1,1) PRIMARY KEY,
    user_id INT NOT NULL,
    name NVARCHAR(100) NOT NULL,
    monthly_budget DECIMAL(10, 2) NOT NULL,
    CONSTRAINT FK_ExpenseCategories_Users FOREIGN KEY (user_id)
        REFERENCES users(id)
        ON DELETE CASCADE
);

CREATE NONCLUSTERED INDEX IX_ExpenseCategories_UserID ON expense_categories (user_id);

CREATE TABLE expenses (
    id INT IDENTITY(1,1) PRIMARY KEY,
    user_id INT NOT NULL,
    category_id INT NOT NULL,
    name NVARCHAR(100),
    amount DECIMAL(10,2) NOT NULL,
    date DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_Expenses_Users FOREIGN KEY (user_id) REFERENCES users(id),
    CONSTRAINT FK_Expenses_Categories FOREIGN KEY (category_id)
        REFERENCES expense_categories(id)
        ON DELETE CASCADE
);

CREATE NONCLUSTERED INDEX IX_Expenses_UserID ON expenses (user_id);
CREATE NONCLUSTERED INDEX IX_Expenses_CategoryID ON expenses (category_id);

CREATE TABLE refresh_tokens (
    id INT IDENTITY(1, 1) PRIMARY KEY,
    token NVARCHAR(250) NOT NULL,
    expires DATETIME2 NOT NULL,
    created_at DATETIME2 DEFAULT GETDATE() NOT NULL,
    created_by_ip NVARCHAR(250) NOT NULL,
    revoked BIT NOT NULL DEFAULT 0,
    replaced_by_token NVARCHAR(250),
    session_expires_at DATETIME2 NOT NULL DEFAULT DATEADD(HOUR, 12, GETDATE()),
    user_id INT NOT NULL,
    CONSTRAINT FK_REFRESH_TOKEN_USERS FOREIGN KEY (user_id)
        REFERENCES users(id)
        ON DELETE CASCADE
);
