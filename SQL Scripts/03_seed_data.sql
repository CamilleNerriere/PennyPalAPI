USE PennyPal;

----------------------------------------------------------
-- 👤 Utilisateur 1 : John Doe
----------------------------------------------------------

INSERT INTO users (lastname, firstname, email)
VALUES ('Doe', 'John', 'john.doe@example.com');

INSERT INTO auth (email, password_hash, password_salt)
VALUES ('john.doe@example.com', 0x67A7656818C5D95D327F525B4F8D1C484CF5C8B1F535C537C5D714EC775B56C9, 0x709043EA5E9AE9AAE32F8CFC782E1207);

-- Catégories
INSERT INTO expense_categories (user_id, name, monthly_budget)
VALUES 
(1, 'Alimentation', 300.00),
(1, 'Transport', 100.00);

-- Dépenses
INSERT INTO expenses (user_id, category_id, amount, date)
VALUES 
(1, 1, 45.50, '2025-03-01 08:30:00'),
(1, 2, 20.00, '2025-03-02 14:00:00');

----------------------------------------------------------
-- 👤 Utilisateur 2 : Jane Smith
----------------------------------------------------------

INSERT INTO users (lastname, firstname, email)
VALUES ('Smith', 'Jane', 'jane.smith@example.com');

INSERT INTO auth (email, password_hash, password_salt)
VALUES ('jane.smith@example.com',  0x67A7656818C5D95D327F525B4F8D1C484CF5C8B1F535C537C5D714EC775B56C9, 0x709043EA5E9AE9AAE32F8CFC782E1207);

-- Catégorie
INSERT INTO expense_categories (user_id, name, monthly_budget)
VALUES (2, 'Loisirs', 150.00);

-- Dépense
INSERT INTO expenses (user_id, category_id, amount, date)
VALUES (2, 3, 75.00, '2025-03-03 19:45:00');

----------------------------------------------------------
-- 👤 Utilisateur 3 : Charlie Brown
----------------------------------------------------------

INSERT INTO users (lastname, firstname, email)
VALUES ('Brown', 'Charlie', 'charlie.brown@example.com');

INSERT INTO auth (email, password_hash, password_salt)
VALUES ('charlie.brown@example.com',  0x67A7656818C5D95D327F525B4F8D1C484CF5C8B1F535C537C5D714EC775B56C9, 0x709043EA5E9AE9AAE32F8CFC782E1207);

-- Catégories
INSERT INTO expense_categories (user_id, name, monthly_budget)
VALUES 
(3, 'Santé', 200.00),
(3, 'Logement', 800.00);

-- Dépenses
INSERT INTO expenses (user_id, category_id, amount, date)
VALUES 
(3, 4, 120.00, '2025-03-04 10:15:00'),
(3, 5, 600.00, '2025-03-05 18:00:00');

----------------------------------------------------------
-- 👤 Utilisateur 4 : Demo User
----------------------------------------------------------

INSERT INTO users (lastname, firstname, email)
VALUES ('Demo', 'User', 'demo.user@example.com');

INSERT INTO auth (email, password_hash, password_salt)
VALUES ('demo.user@example.com',  0x67A7656818C5D95D327F525B4F8D1C484CF5C8B1F535C537C5D714EC775B56C9, 0x709043EA5E9AE9AAE32F8CFC782E1207);

-- Catégories
INSERT INTO expense_categories (user_id, name, monthly_budget)
VALUES 
(4, 'Courses', 250.00),
(4, 'Loisirs', 180.00),
(4, 'Transport', 120.00),
(4, 'Abonnements', 60.00);

-- Dépenses
INSERT INTO expenses (user_id, category_id, amount, date)
VALUES 
(4, 6, 58.90, '2025-04-01'),
(4, 6, 24.50, '2025-04-02'),
(4, 7, 12.00, '2025-03-30'),
(4, 8, 40.00, '2025-03-28'),
(4, 8, 28.00, '2025-03-20'),
(4, 9, 9.99, '2025-03-15'),
(4, 9, 7.99, '2025-03-01');
