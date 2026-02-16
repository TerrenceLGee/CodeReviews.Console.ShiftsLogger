# Shifts Logger 

Console application written with C# 14/.NET 10 using Microsoft's Visual Studio Community.

This is a application that allows a employee to manage the shifts that they work at their current place of employment.

Created following the curriculm of [C# Academy](https://www.thecsharpacademy.com/)

[Shifts Logger](https://www.thecsharpacademy.com/project/17/shifts-logger)

## Features	
- Allows an employee to create, update, read, and delete their work shifts.
- Implements user/employee creation as well as login/logout functionality.
- Uses ASP.NET Core Identity for user creation and management.
- Uses password authentication.
- Use Jwt and Refresh Tokens for Authorization.
- Uses the Result pattern from the service layer.
- Uses a custom Api Response from the controllers.
- Seeds the database with both a default admin user (for testing the one admin endpoint) as well as a default user and shifts.
- Unit tests for the shifts logger service layer.

## Explanation Of Certain Choices Made For This Project
User creation and authentication as well as Jwt and Refresh Tokens for Authorization were not a part of the project requirements.
The reason I choose to implement these functionalities in this project were for learning purposes only. It is my sincere desire that I implemented these
functionalities properly. I also want to push myself to higher and higher levels. You will also notice that I have the Jwt key in the appsettings.json configuration file. That is obviously not secure, but because this was not part of the project requirements I felt it best to leave the key in the configuration file instead of forcing the reviewer/end-user to attempt to generate their own Jwt key. Also in regards to users I just implemented basic login/logout functionality. I did not implement a change password functionality though looking back maybe I should have but I did not want to spend even more time on an aspect of this project that was not required.

## Challenges Face When Implementing This Project
- Learning how to develop an ASP.NET Core Web Api. I spent roughly two weeks researching and reading how to implement an ASP.NET Core web api.
- Getting the API to work as I wanted it to. There were many iterations of this project before I felt confident enough to submit it for review.
- Learning about the Result pattern and also creating custom API Responses.
- The biggest challenge was in my choice to implement ASP.NET Core Identity and Token Authorization. I went through many written tutorials and documentation. I realize that it will take much more learning and practice to use ASP.NET Core identity to it's full potential.

## What I Learned Implementing This Project
- I learned so much in regards to creating a ASP.NET Core web api, such as using controllers for endpoints as well as HttpStatus codes and Http Actions.
- I learned about ASP.NET Core Identity and JWT Tokens and Refresh Tokens, knowledge that I hope to expand upon because there is so much more to learn.
- The most important thing that I learned is that I have so much to learn 😉

# Instructions
In order to create and seed the database you must go into the appsettings.json file in the ShiftsLogger.Api project and enter the SQL Server database connection string for example like the following:
```
"ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=ShiftsLogger-Db;Trusted_Connection=True;"
  }
```

In order to use the program properly you will have run both the API and the Console application simultaneously. The most basic way is from the command line interface. This is also necessary if you are using Visual Studio Code.
It's best to open two terminal windows: One in the API project root directory and other in the Console project root directory. For the API root directory run the following commands:
```
dotnet build && dotnet run --launch-profile https
```
Then from the Console project root directory
```
dotnet build && dotnet run
```

For Visual Studio: The following link will take you to instructions on starting multiple projects in Visual Studio:

[Start multiple projects in Visual Studio](https://learn.microsoft.com/en-us/visualstudio/ide/how-to-set-multiple-startup-projects?view=visualstudio)

For Jetbrains Rider: The following link will take you to instructions on starting multiple projects in Jetbrains Rider:

[Start multiple projects in Jetbrains Rider](https://www.jetbrains.com/help/rider/Run_Debug_Multiple.html)

Once the Console application starts (after the API has finished loading) you can either create a register a new user and then create shifts yourself or you can use the either the default user or the default admin user (which you can use to test the admin endpoint, the admin user however does not have any shifts logged you would hace to create new shifts as the admin user for that)
The default admin user is:
```
username: admin@example.com
password: Pa$$w0rd
```

The default user is:
```
username: gramsay@example.com
password: Pa$$w0rd
```

## Areas To Improve Upon
- Everything really, obviously this is the first project in which you implement your own API so that is probably the most important thing to imrpove upon at this juncture.

## Technolgies Used
- [Spectre.Console](https://spectreconsole.net/)
- [Serilog](https://serilog.net/)
- [XUnit](https://xunit.net/?tabs=cs)
- [Moq](https://github.com/devlooped/moq)
- [Microsoft.EntityFrameworkCore](https://learn.microsoft.com/en-us/ef/)
- [Microsoft SQL Server](https://learn.microsoft.com/en-us/sql/?view=sql-server-ver17)
- [ASP.NET Core Identity](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-10.0&tabs=visual-studio)

## Helpful Resources Used
- [DotNetTutorials.net Web API Tutorials](https://dotnettutorials.net/course/asp-net-core-web-api-tutorials/)
- [How to Customize ASP.NET Core Identity With EF Core](https://antondevtips.com/blog/how-to-customize-aspnet-core-identity-with-efcore-for-your-project-needs)