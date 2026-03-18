CREATE TABLE
  user.user_accounts (
    id INTEGER PRIMARY KEY REFERENCES user.users_details (id) ON DELETE CASCADE,
    password_hash TEXT NOT NULL
  );

INSERT INTO
  user.user_accounts (id, password_hash)
SELECT
  id,
  password_hash
FROM
  user.users_details;

ALTER TABLE user.users_details
DROP COLUMN password_hash;