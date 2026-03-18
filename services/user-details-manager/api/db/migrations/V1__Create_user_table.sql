CREATE SCHEMA IF NOT EXISTS user_details_manager;

CREATE TABLE
  IF NOT EXISTS user_details_manager.user_details (
    id INTEGER PRIMARY KEY,
    first_name TEXT NOT NULL,
    last_name TEXT NOT NULL
  );