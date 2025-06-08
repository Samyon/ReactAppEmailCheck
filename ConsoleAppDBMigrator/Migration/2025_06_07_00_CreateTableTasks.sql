CREATE TABLE tasks
(
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    created_at TEXT DEFAULT CURRENT_TIMESTAMP,--datetime
    email TEXT,
    status INTEGER NOT NULL,
        --0 just write
        --1 in progress send code
        --2 done send code
        --3 code verified
    change_status_at TEXT NOT NULL,--datetime
    code TEXT,
    ip_client TEXT, --for antiddos
    session TEXT    --for antiddos

);



--индекс не нужен