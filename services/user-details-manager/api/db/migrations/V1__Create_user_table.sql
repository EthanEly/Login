CREATE SCHEMA IF NOT EXISTS user_details;

CREATE TABLE
  user_details.users (
    id SERIAL PRIMARY KEY,
    email TEXT NOT NULL UNIQUE,
    first_name TEXT NOT NULL,
    last_name TEXT NOT NULL,
    password_hash TEXT NOT NULL
  );