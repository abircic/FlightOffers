# Flight Offers - Low-Budget Flight Search

Welcome to the Flight Offers application! This application allows you to search for low-budget flights to find the most affordable options for your travel needs.

Here are the steps to run both the backend and frontend parts of the application:

## Backend

Before running the backend, you must add the appsettings.json file with the required configuration. Please follow these steps:
1. In the root directory of the FlightOffers.Api project, create a new file named appsettings.json.
   
appsettings.json example file:
```
{
  "DatabaseSettings": {
    "ConnectionString": "Server=localhost;Port=5432;Username=postgres;Password=postgres;Database=flight_offers"
  },
  "AppSettings": {
    "FlightOffersBaseUrl": "https://test.api.amadeus.com",
    "ApiKey": "rb4SJj2jCQ7w6vGUBxJgJ02jSARut9UY",
    "ApiSecretKey": "tJGRCwykO2SsJgUK"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}

```
2. Save the appsettings.json file.
   
The backend of the application uses a PostgreSQL database for storing flight data.
Open the terminal and navigate to the directory where the FlightOffers.Migrations file is located.
Execute the migration to create the necessary tables in the database:
#### `dotnet run project FlightOffers.Migrations.csproj "connectionString=Server=localhost;Port=5432;Username=postgres;Password=postgres;Database=flight_offers`
Open the terminal and navigate to the directory where the FlightOffers.Api file is located.
Now, the backend is configured, and you can start it with:
#### `dotnet run`
The backend will start on your local server at the default address https://localhost:5000.

## Frontend

Before running the frontend part, ensure that you have Node.js and npm installed on your computer, then you need to add a .env file with the required configuration. 
Please follow these steps:

1. In the root directory create a new file named .env.
   
.env example file:
``
REACT_APP_API_BACKEND_URL=http://localhost:5000
``

2. Save the .env file.
   
The frontend part of the application needs the REACT_APP_API_BACKEND_URL variable to know the backend API URL it should communicate with.

Open the terminal and navigate to the directory where the package.json file is located.
Install the required packages by running the following command:
### `npm install`
Start the frontend application with the following command:
### `npm start`
The frontend will start on your local server at the default address https://localhost:3000.

Display example:

<img width="1000" alt="Display Example" src="/Client/screenshots/DisplayExample.png">
