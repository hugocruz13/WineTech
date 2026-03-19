#!/bin/sh
set -e

DB_HOST="${DB_HOST:-mssql}"
DB_NAME="${DB_NAME:-ISI}"
DB_USER="${DB_USER:-sa}"
DB_PASSWORD="${DB_PASSWORD:-Passw0rd_dev!}"

CONNECTION_STRING="Server=${DB_HOST};Database=${DB_NAME};User Id=${DB_USER};Password=${DB_PASSWORD};TrustServerCertificate=True;"
ESCAPED_CONNECTION_STRING=$(printf '%s\n' "$CONNECTION_STRING" | sed -e 's/[\/&]/\\&/g')

sed -i "s#Server=localhost;Database=ISI;User Id=sa;Password=Passw0rd_dev!;TrustServerCertificate=True;#${ESCAPED_CONNECTION_STRING}#g" /app/Web.config

exec xsp4 --port 8080 --nonstop --root /app
