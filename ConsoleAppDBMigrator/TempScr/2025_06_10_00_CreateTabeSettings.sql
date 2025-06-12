CREATE TABLE settings
(
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    created_at TEXT DEFAULT CURRENT_TIMESTAMP,--datetime
    setting_key TEXT,
    setting_value TEXT

);



--индекс не нужен