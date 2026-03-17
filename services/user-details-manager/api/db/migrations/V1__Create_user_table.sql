CREATE SCHEMA IF NOT EXISTS user;

CREATE TABLE
  user.users_details (
    id SERIAL PRIMARY KEY,
    email TEXT NOT NULL UNIQUE,
    first_name TEXT NOT NULL,
    last_name TEXT NOT NULL,
    password_hash TEXT NOT NULL
  );