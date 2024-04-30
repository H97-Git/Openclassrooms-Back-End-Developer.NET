# Diabetes Prediction Microservice Project
Welcome to the Diabetes Prediction Microservice Project! This project aims to develop a set of microservices to predict diabetes based on patient records. The microservices will be deployed using Docker containers and will interact with each other to generate the final diabetes report.

## Project Overview
The project is divided into three sprints, each focusing on specific functionalities and tasks. The main goal of the first sprint is to set up the infrastructure for building Docker microservices and implement a REST API for accessing patient records. Subsequent sprints will focus on enhancing the functionality and integrating additional features.

## Sprints Overview
### Sprint 1
* Objective: Set up Docker microservices infrastructure and implement REST API for patient records.
* Tasks:
* Set up Docker environment.
* Develop REST API endpoints for CRUD operations on patient records.
* Implement data layer to interact with SQL database.
* Deliverables:
* Updated Kanban board with sprint tasks.
* Completed retrospective template for Sprint 1.
* Code project and Dockerfiles in GitHub repository.
* Documentation for the REST service(s).
### Sprint 2
* Objective: Enhance functionality and prepare for integration with other microservices.
* Tasks:
* Implement additional endpoints for data manipulation.
* Refactor code for improved performance and scalability.
* Write unit and integration tests.
* Deliverables:
* Updated Kanban board reflecting sprint progress.
* Completed retrospective template for Sprint 2.
* Test report for unit and integration tests.
* Documentation updates reflecting changes made in Sprint 2.
### Sprint 3
* Objective: Finalize functionality and prepare for final presentation to the client.
* Tasks:
* Integrate microservices to generate the diabetes prediction report.
* Conduct comprehensive testing and debugging.
* Prepare walkthrough demo and slide deck for final presentation.
* Deliverables:
* Final version of the microservice codebase.
* Completed retrospective template for Sprint 3.
* Walkthrough demo of the final product.
* Slide deck for final presentation.
## Presentation
During the presentation, you will provide a demo of the final working product and discuss your methodology and deliverables with Taylor, who will act as the project manager. Be prepared to defend your decisions and discuss any challenges encountered during the project.


# P10 OpenClassrooms
A medical solution to Insert, View, Edit and Calculate data about Patients

## USE
[Visual Studio ( or VSCode)](https://visualstudio.microsoft.com/)

With Docker running :
```
1. Download the project ( git clone https://github.com/H97-Git/q24uBYLBA1VjJuMAQ4eok.git Path )
2. Open the project ( *.sln )
3. Build the solution.
3. Make sure 'docker-compose(.yml)' is set a start-up project.
4. Wait Container Tools.
5. CTRL + F5
```
## Patient
Microservice to Create, Edit and Get Patient data (SQL Server) :
- Docker
- ASP.NET Core Web API
- MS SQL Server
- Serilog
- Swagger
## PatientNotes/History
Microservice to Create, Edit and Get PatientNotes/History data : 
- Docker
- ASP.NET Core Web API
- MongoDb
- Swagger
- Serilog
## DiabetesRiskLevel
Microservice to calculate the diabetes risk level of a patient. With datas from Patient & PatientNotes/History
- Docker
- ASP.NET Core Web API
- Swagger
- Serilog
## BlazorPatient
Simple .NET Core Blazor application connected to all services above.
To ship something deliverable.

## Docker Links (when containerized)
- Blazor : [http://localhost:6000/](http://localhost:6000/)
- Patient : [http://localhost:5000/swagger/index.html](http://localhost:5000/swagger/index.html)
- Notes : [http://localhost:5002/swagger/index.html](http://localhost:5002/swagger/index.html)
- Assessment : [http://localhost:5004/swagger/index.html](http://localhost:5004/swagger/index.html)
