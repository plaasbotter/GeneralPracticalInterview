# AculocityPracticalInterview

#SQL SERVER
Esiest to run a docker image
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Password1234" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest
Point the app settings to the database
The program will construct the tables as needed
