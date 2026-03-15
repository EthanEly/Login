CREATE SCHEMA IF NOT EXISTS user_details;

CREATE TABLE
  user_details."Users" (
    "Id" SERIAL PRIMARY KEY,
    "Email" TEXT NOT NULL UNIQUE,
    "FirstName" TEXT NOT NULL,
    "LastName" TEXT NOT NULL,
    "PasswordHash" TEXT NOT NULL
  );