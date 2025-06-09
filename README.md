# CustomerApp

CustomerApp is a multi-project ASP.NET Core solution I built to manage customer data and business logic in a clean, modular way. It’s structured to keep things organized — separating the core domain, data access, business services, and tests into their own projects. This makes the app easier to maintain, test, and scale.

## Project Structure

Here’s what each part of the solution does:

- CustomerApp – The main application (MVC or Web API).
- CustomerApp.Data – Handles database context and EF Core-related logic.
- CustomerApp.Domain – Contains all the core models and domain entities.
- CustomerApp.Services – Business logic and service layer.
- CustomerApp.Tests – Unit tests to ensure everything works as expected.

## Getting Started

To run this project locally:

1. Clone the repository:  
   git clone https://github.com/Azubikeamala/Customer-App.git  
   cd Customer-App

2. Open the solution:  
   Open `CustomerApp.sln` in Visual Studio or VS Code (with the C# extension installed).

3. Restore dependencies:  
   dotnet restore

4. Run the application:  
   dotnet run --project CustomerApp

## Running Tests

To run tests:  
dotnet test CustomerApp.Tests

## Tech Stack

- ASP.NET Core  
- C#  
- Entity Framework Core  
- xUnit (for testing)  
- Clean Architecture principles

## Future Improvements

- Add user authentication  
- Improve test coverage  
- Add Swagger for API documentation  
- Deploy to Azure or Render  
- Export data to CSV or PDF

## License

This project is licensed under the MIT License.

Thanks for checking it out!
