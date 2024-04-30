# Californian Health Web Platform Enhancement Project
Welcome to the Californian Health Web Platform Enhancement Project! This project aims to address critical issues identified in the current web platform of Californian Health, a leading private healthcare provider.

## Project Overview
The current web platform of Californian Health is facing several issues, including slow access, unresponsive booking systems, and inaccurate consultant availability information. As a senior software developer, your task is to propose architectural improvements, refactor code, and implement enhancements to resolve these issues.

## Getting Started
Clone or download the repository to your local machine to get started. Ensure that you have the necessary tools and frameworks installed to run and develop the web application.

## Deliverables
1. Functional and Technical Documentation:
   * Completed documentation outlining the revised architecture of the system and detailing the proposed solutions.
   * Test coverage report demonstrating the completeness of the implemented enhancements.
2. Updated Codebase:
   * Link to a personal GitHub repository containing the updated codebase.
## Presentation
During the presentation, you'll provide an overview of your project deliverables and explain the following:

* Your process for developing and testing the refactored version of Californian Health's architecture.
* Implementation details of the proposed changes and enhancements.
* Tests conducted and reasons behind them, including unit and integration tests.
* Challenges encountered during development and mitigation strategies.
* Reflections on what could have been done differently.
* Assessment Guidelines
* Complete a comprehensive unit and integration test suite to account for the implemented changes.
* Fix faults reported by the customer on the application, addressing critical issues affecting user experience.
* Improve the application based on customer requests, ensuring scalability, reliability, and performance.
* Produce technical and functional documentation to aid in understanding the proposed solutions and changes.


# P8 OpenClassrooms
A solution to allow users to create booking with doctor.

## USE
[Visual Studio ( or VSCode)](https://visualstudio.microsoft.com/)

[Jetbrains.com Rider](https://www.jetbrains.com/rider/)

With Docker running :
```
1. Download the project ( git clone https://github.com/H97-Git/url Path )
2. Open the project ( *.sln )
3. Build the solution.
3. Make sure 'docker-compose(.yml)' is set a start-up project.
4. Wait Container Tools.
5. CTRL + F5
```
## With :
- Docker
- ASP.NET Core Web API
- MS SQL Server
- Serilog
- Swagger
- RabbitMQ
- Ocelot

## BlazorPatient
Simple .NET Core Blazor application connected to all services above.
To ship something deliverable.

## Docker Links (when containerized)
- Blazor : [http://localhost:8080/](http://localhost:8080/)
- Gateway : [http://localhost:7000/swagger/index.html](http://localhost:7000/swagger/index.html)
- Booking : [http://localhost:6000/swagger/index.html](http://localhost:6000/swagger/index.html)
- Demographics : [http://localhost:5000/swagger/index.html](http://localhost:5000/swagger/index.html)
