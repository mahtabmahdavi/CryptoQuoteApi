# Crypto Quote API 💰

A .NET-based cryptocurrency quote API that provides real-time cryptocurrency prices and exchange rates. This project is built using .NET 9 and developed in Visual Studio, implementing clean architecture principles and best practices for API development.

## Architecture Overview

The project follows Clean Architecture principles and is organized into the following layers:

### Api Layer

- **Controllers**: Handles HTTP requests and responses for cryptocurrency quotes
- **Middlewares**: Implements cross-cutting concerns like error handling and request validation

### Application Layer

- **DTOs**: Data Transfer Objects for request/response models
- **Interfaces**: Defines contracts for external dependencies
- **Validators**: Handles input validation and business rules
- **Settings**: Application configuration and settings
- **Exceptions**: Custom application exceptions

### Domain Layer

- **Models**: Core business entities and domain models

### Infrastructure Layer

- **Services**: External service implementations
- **Models**: Infrastructure-specific models and DTOs

### Tests Layer

- Unit tests for cache service

The application integrates with two external APIs:

- **CoinMarketCap API**: Provides real-time cryptocurrency prices, market data, and historical information
- **ExchangeRates API**: Handles currency conversion rates and exchange rate calculations

## How to Run

### Prerequisites

- .NET 9 SDK
- Visual Studio 2022 or later
- API keys for CoinMarketCap and ExchangeRates

### Setup

1. Clone the repository
2. Open the solution in Visual Studio
3. Update the API keys in `appsettings.json`:

   ```json
   "ExternalApis": {
     "CoinMarketCap": {
       "ApiKey": "YOUR_COINMARKETCAP_API_KEY"
     },
     "ExchangeRates": {
       "ApiKey": "YOUR_EXCHANGERATES_API_KEY"
     }
   }
   ```

### Running the Application

1. Open terminal in the project directory
2. Run the following commands:

   ```bash
   dotnet restore
   dotnet build
   dotnet run
   ```

3. The API will be available at `https://localhost:5001` or `http://localhost:5000`

### Running Tests

1. Open terminal in the project directory
2. Run the following command to execute all tests:

   ```bash
   dotnet test
   ```

# Answers to Technical Questions

**1. How long did you spend on the coding assignment? What would you add to your solution if you had more time?**
I spent approximately 20 hours on this project. If I had more time, I would add integration tests for better test coverage.

**2. What was the most useful feature that was added to the latest version of your language of choice?**
Recently, I've been making extensive use of Primary Constructors in .NET, which is a feature that helps reduce boilerplate code in class definitions. Here's an example:

```csharp
public class CryptoQuoteService(ICryptoRepository repository, ILogger<CryptoQuoteService> logger)
{
    private readonly ICryptoRepository _repository = repository;
    private readonly ILogger<CryptoQuoteService> _logger = logger;

    // Service methods...
}
```

**3. How would you track down a performance issue in production? Have you ever had to do this?**
I think the most effective approach would be to implement a comprehensive logging strategy. No, I haven't had the opportunity to track down performance issues in a production environment yet.

**4. What was the latest technical book you have read or tech conference you have been to? What did you learn?**
While I haven't recently attended any technical conferences or read specific technical books, I maintain my knowledge through:

- Online documentation and tutorials
- Practical project experience
- Following industry best practices and patterns

**5. What do you think about this technical assessment?**
This technical assessment is particularly valuable because it simulates a real-world scenario that many financial applications face.

**6. Please, describe yourself using JSON.**

```json
{
  "name": "Mahtab Mahdavi",
  "age": 28,
  "education": {
    "degree": "Computer Engineering",
    "field": "Software Engineering"
  },
  "technicalSkills": {
    "languages": ["C#"],
    "frameworks": ["ASP.NET Core"],
    "databases": ["Entity Framework"],
    "architecture": ["Clean Architecture", "Microservices"]
  },
  "interests": [
    "API Development",
    "Software Architecture",
    "Clean Code",
    "Best Practices"
  ]
}
```
