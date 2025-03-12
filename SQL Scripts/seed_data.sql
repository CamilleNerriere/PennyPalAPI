USE PennyPal;
GO

INSERT INTO users (lastname, firstname, email)
VALUES 
('Doe', 'John', 'john.doe@example.com'),
('Smith', 'Jane', 'jane.smith@example.com'),
('Brown', 'Charlie', 'charlie.brown@example.com');

INSERT INTO auth (email, password_hash, password_salt, role)
VALUES 
('john.doe@example.com', 0x123456, 0xabcdef, 'admin'),
('jane.smith@example.com', 0x654321, 0xfedcba, 'user'),
('charlie.brown@example.com', 0xa1b2c3, 0xd4e5f6, 'user');

INSERT INTO expense_categories (user_id, name, monthly_budget)
VALUES 
(1, 'Alimentation', 300.00),
(1, 'Transport', 100.00),
(2, 'Loisirs', 150.00),
(3, 'Santé', 200.00),
(3, 'Logement', 800.00);

INSERT INTO expenses (user_id, category_id, amount, date)
VALUES 
(1, 1, 45.50, '2025-03-01 08:30:00'),
(1, 2, 20.00, '2025-03-02 14:00:00'),
(2, 3, 75.00, '2025-03-03 19:45:00'),
(3, 4, 120.00, '2025-03-04 10:15:00'),
(3, 5, 600.00, '2025-03-05 18:00:00');


