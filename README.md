# NTR21Z-Yatskevich-Illia

### To run this you need to have installed
- `.NET 6`
- `Entity Framework Core 6`
- `node v16.13.1`
- `docker`

## Step 1: Create database
run `docker compose up --build`
to create database at port 3306 and phpmyadmin at port 8080

## Step 2: Populate database
run `dotnet ef database update`
to populate database with data

## Step 3: Publish application and it's dependencies for deployment
run `dotnet publish -c Release`
to create a framework-dependent cross-platform binary for the project in the `/bin/Release/net6.0/publish` directory

## Step 4: Run application
cd to `/bin/Release/net6.0/publish` and run `./Timereporter`