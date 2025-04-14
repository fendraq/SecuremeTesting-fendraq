CREATE TYPE user_role AS ENUM ('admin', 'customer_support');
CREATE TYPE case_status AS ENUM ('unopened', 'open', 'closed');
CREATE TYPE case_category AS ENUM ('shipping', 'payment', 'product', 'other');

CREATE TABLE "users"(
    "id" SERIAL PRIMARY KEY NOT NULL,
    "role" user_role NULL,
    "user_name" VARCHAR(255) NULL,
    "password" VARCHAR(255) NULL,
    "email" VARCHAR(255) NULL,
    "active" BOOLEAN NULL
);

CREATE TABLE "cases"(
    "id" SERIAL PRIMARY KEY NOT NULL,
    "status" case_status NULL DEFAULT 'unopened',
    "category" case_category NULL DEFAULT 'other',
    "title" VARCHAR(255) NULL,
    "customer_first_name" VARCHAR(255) NULL,
    "customer_last_name" VARCHAR(255) NULL,
    "customer_email" VARCHAR(255) NULL,
    "case_opened" TIMESTAMP(0) WITHOUT TIME ZONE NULL,
    "case_closed" TIMESTAMP(0) WITHOUT TIME ZONE NULL,
    "case_handler" BIGINT NULL
);


CREATE TABLE "messages"(
    "id" SERIAL PRIMARY KEY NOT NULL,
    "case_id" BIGINT NULL,
    "message" TEXT NULL,
    "timestamp" TIMESTAMP(0) WITHOUT TIME ZONE NULL,
    "is_sender_customer" BOOLEAN NOT NULL DEFAULT 'true'
);

ALTER TABLE
    "cases" ADD CONSTRAINT "cases_case_handler_foreign" FOREIGN KEY("case_handler") REFERENCES "users"("id");
ALTER TABLE
    "messages" ADD CONSTRAINT "messages_case_id_foreign" FOREIGN KEY("case_id") REFERENCES "cases"("id");
ALTER TABLE messages ALTER COLUMN timestamp SET DEFAULT now();
ALTER TABLE users ADD COLUMN status VARCHAR(255) DEFAULT 'pending';

CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

ALTER TABLE cases ADD COLUMN chat_token UUID DEFAULT uuid_generate_v4();