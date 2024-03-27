# Flashcards
Create stacks of flashcards and study them! At the end check your study report and see your scores improve over the course of the year.

## SQL Server Setup on Mac
First download Docker and Azure Data Studio.

Pull docker image:
```bash
docker pull mcr.microsoft.com/mssql/server:2022-latest
```

Run SQL Server
```bash
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=123456aA" \
   -p 1433:1433 --name sql1 --hostname sql1 -d \
   mcr.microsoft.com/mssql/server:2022-latest
```

### Optional steps
Stopping and starting
```bash
docker stop sql1
docker start sql1
```

Connect to SQL Server
```bash
docker exec -it sql1 "bash"
opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P "123456aA"
```

Create DB
```bash
CREATE DATABASE Flashcards;
SELECT Name from sys.databases;
```

In Azure Data Studio use connection string: `Data Source=localhost; Initial Catalog=TestDb; User id=SA; password=123456aA;`.

Initialize tables with a script
```bash
docker cp init.sql sql1:/tmp
docker exec -it sql1 bash
/opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P '123456aA' -i /tmp/init.sql
```