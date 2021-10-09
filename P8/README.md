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
