# ReadingIsGoodService

<b>Customer Api</b>:

https://reading-is-good-customers.herokuapp.com/swagger

![image](https://user-images.githubusercontent.com/17797666/166956538-e265f016-cb6c-4ef6-a608-d91fdd0b7125.png)

<b>Orders Api</b>:

https://reading-is-good-orders.herokuapp.com/swagger

![image](https://user-images.githubusercontent.com/17797666/166949637-d7427f3a-02db-495b-9a95-5b9bda53bce8.png)

<b>Database</b>

Schema:
![image](https://user-images.githubusercontent.com/17797666/147704668-fb9f6c2b-6b36-475d-9fda-9f826110f85a.png)


Script: https://github.com/tumanina/ReadingIsGoodService/blob/main/ReadingIsGoodDb.sql

<b>Authorizaion and Authentification:</b>

Token for testing purpose: eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyLCJ1c2VyaWQiOjU3OTN9.mXLWmKSxcuTX_GM5V6e7WeEDA-dofCsr0ZZldkd7v7k

Authorization header: Bearer {token}

Current logic just read token and extract userId without token validation and checking that user exists.

## How to run locally:

### Api

1. Visual Studio (kestrel)

2. Docker compose

`docker-compose up -d`

Customers Api: http://localhost:8031/swagger
Orders Api: http://localhost:8032/swagger

3. Docker CLI

Build image of needed service(s) `docker build -t {name} -f {path} .`

`docker docker build -t customers-api -f ReadingIsGoodService.Customers.Api/Dockerfile .`

`docker docker build -t orders-api -f ReadingIsGoodService.Orders.Api/Dockerfile .`

Run:

`docker run -p 8031:80 customers-api`

`docker run -p 8032:80 orders-api`

### Database:

Database access layer presents EntityFrameworkCore code first approach that provides mechanism to deploy a new database and helps keep concistency between code and database structure. 

Application provides 2 options to use database:
1. Physical database. 
To create/update local database change `ConnectionStrings.DefaultConnection` setting in appsetting.json file to correct server and execute command 'update-database' in PackageManagerConsole or other CLI.  Also functionality can be easily extended to execute migrations on application start (https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/applying?tabs=dotnet-core-cli#apply-migrations-at-runtime).
Update database command can be called as one of deployment's steps or part of container (Dockerfile).
2. Database in memory. Switching to this option is managed by flag UseInMemoryDatabase in appsetting.json file. This features can be used for testing purposes (integration tests).

<b>Assumptions:</b>
1. Authentification (bearer token) part is implemented as a draft without token and user validation, authorization part (roles, ownership) is fully missed, you can find a few variants of authentification and authorization in my other project https://github.com/tumanina/AuthService. 
2. Some of user input validation was not implemented (email validation, length of name and etc).
3. Only creating order functionality was covered by unit and integration tests. In real application each condition and edge cases must be covered by unit and integration tests.
4. It is good to use page size and page number parameters in api request.
5. Some part of the system can be done other way: for example in case of events based architecture activity logging can be done based on these events, also Saga pattern can be used to solve problem with stock change in order creating functionality.
6. Activity tracker implemented only for add and update, for delete it is good to implemement soft delete (IsDeleted flag in entities) and call update method of BaseRepository.
7. Database design represents very simple solution, for example address has been simplified from one-to-many relation to single field, also price should contain currency and have to be time-related.

<b>Stock update problem</b> can also be solved in other ways:
1. Transaction on database level with commit only after all operations done
2. Saga patern with rollback operation in case of microservices implementation
3. Comand pattern with Undo operation.
