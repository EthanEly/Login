CREATE TABLE
  IF NOT EXISTS user.user_accounts (
    id INTEGER PRIMARY KEY REFERENCES user.users_details (id) ON DELETE CASCADE,
    password_hash TEXT NOT NULL
  );