

--Это итоговая таблица проверенных Email
CREATE TABLE confirmed_emails
(
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    created_at TEXT DEFAULT CURRENT_TIMESTAMP,--datetime
    email TEXT UNIQUE

);


