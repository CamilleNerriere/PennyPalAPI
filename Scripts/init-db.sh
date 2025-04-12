echo "Waiting for SQL Server"
sleep 15

echo "Create Database"
/opt/mssql-tools/bin/sqlcmd -S $MSSQL_SERVER -U $MSSQL_USER -P $MSSQL_PASSWORD -d master -i /tmp/sql/01_create_database.sql

echo "Create table"
/opt/mssql-tools/bin/sqlcmd -S $MSSQL_SERVER -U $MSSQL_USER -P $MSSQL_PASSWORD -d master -i /tmp/sql/02_create_tables.sql

echo "Seed tables"
/opt/mssql-tools/bin/sqlcmd -S $MSSQL_SERVER -U $MSSQL_USER -P $MSSQL_PASSWORD -d master -i /tmp/sql/03_seed_data.sql

if [ $? -ne 0 ]; then
  echo " Failed to run sql scripts"
  exit 1
fi

echo "Table created and seeded"