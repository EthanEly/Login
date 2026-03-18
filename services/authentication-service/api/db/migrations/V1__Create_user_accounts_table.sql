CREATE SCHEMA IF NOT EXISTS authentication_service;

CREATE TABLE
  IF NOT EXISTS authentication_service.user_accounts (
    id SERIAL PRIMARY KEY,
    email TEXT NOT NULL UNIQUE,
    password_hash TEXT NOT NULL
  );