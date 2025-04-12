IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'PennyPal')
BEGIN
    CREATE DATABASE PennyPal;
END
