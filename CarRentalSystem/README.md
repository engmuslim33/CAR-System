# Car Rental System (ASP.NET Core Web API)

A simple car rental API using:
- ASP.NET Core Web API
- Entity Framework Core
- SQLite (can be switched to SQL Server)

## Features

1. **Cars Management**
   - Add new car
   - Get all cars
2. **Customers Management**
   - Add new customer
3. **Bookings**
   - Create booking
   - Get all bookings
   - Check car availability before booking
4. **Business Logic**
   - Prevent overlapping bookings for the same car
   - Basic total price calculation (`DailyRate * numberOfDays`)

## Project Structure

- `Controllers/`
- `Models/`
- `Data/`
- `DTOs/`
- `Migrations/`

## Setup & Run

### 1) Prerequisites
- .NET 8 SDK

### 2) Restore packages
```bash
dotnet restore
```

### 3) Create database from migration
```bash
dotnet tool install --global dotnet-ef
dotnet ef database update
```

### 4) Run API
```bash
dotnet run
```

Swagger UI should open in development mode.

## Migration Commands

If you change models later:

```bash
dotnet ef migrations add <MigrationName>
dotnet ef database update
```

## Sample Endpoints

### Cars
- `POST /api/cars`
```json
{
  "name": "Toyota Corolla",
  "dailyRate": 65
}
```
- `GET /api/cars`

### Customers
- `POST /api/customers`
```json
{
  "name": "John Doe",
  "phone": "+1-555-1010"
}
```

### Bookings
- `POST /api/bookings`
```json
{
  "carId": 1,
  "customerId": 1,
  "startDate": "2026-04-20",
  "endDate": "2026-04-23"
}
```

- `GET /api/bookings`

- `GET /api/bookings/availability?carId=1&startDate=2026-04-20&endDate=2026-04-23`

## Notes

- Date overlap logic:
  - A new booking conflicts when `newStart <= existingEnd && newEnd >= existingStart`.
- SQLite DB file is `car_rental.db` in project root.
