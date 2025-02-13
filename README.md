# Holiday API Project

This project provides various holiday-related endpoints, including a countries list, holidays grouped by month for a given country and year, the status of a specific day, and the maximum number of consecutive free days for a country and year. The data is fetched from the [Enrico API](https://kayaposoft.com/enrico/) and saved in a database to prevent repeated requests.

[Live version (click)](http://civitta-test-app.westeurope.cloudapp.azure.com/swagger/index.html)

## Endpoints

1. **GET /api/getCountries**  
   Returns a list of all supported countries.

2. **GET /api/getGroupedHolidays**  
   Returns a list of holidays for a given country, year, and month.

3. **GET /api/getDayStatus**  
   Returns the status of a specific day (workday, free day, holiday) for a given country and year.

4. **GET /api/getMaxFreeDaysInRow**  
   Returns the maximum number of consecutive free days (free day + holiday) for a given country and year.

## Technology Stack

- **.NET 9** for API development
- **MSSQL** for database storage
- **Docker** for local development environment
- **Git** for version control (GitHub, GitLab, or Bitbucket)
- **Swagger OpenAPI** for automatically generated API documentation
- **Azure VM** for deployment 
- **GitHub** for ECR

## Setup Instructions

### 1. Clone the repository
```bash
git clone https://github.com/Vladyslavko36/civitta-test-app.git
cd civitta-test-app
```

### 2. Set up the local development environment using Docker
Make sure Docker is installed on your machine.
Create a `.env` file in the project root with the necessary configuration (database connection strings, etc.). Use `.env.example`

### 3. Docker Compose
Run the following command to build and start the containers:
```bash
docker-compose up --build
```

### 4. Database Setup
The database is automatically populated with holiday data by querying the Enrico API on the first run. The data is normalized and stored to avoid repeated requests.

### 5. Run the application
Once the containers are up, the application will be available at `http://localhost:8080`. You can interact with the API and check the OpenAPI documentation at `http://localhost:8080/swagger`.
