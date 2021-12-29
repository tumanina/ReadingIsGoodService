# ReadingIsGoodService

<b>Api</b>:

![image](https://user-images.githubusercontent.com/17797666/147704494-c7475d50-11fc-43b7-96c0-1da69eb1f8bf.png)

<b>Database</b>

Schema:
![image](https://user-images.githubusercontent.com/17797666/147704668-fb9f6c2b-6b36-475d-9fda-9f826110f85a.png)

Database access layer presents EntityFrameworkCore code first approach that provides mechanism to deploy a new database and helps keep concistency between code and database structure. 

Application provides 2 options to use database:
1. Physical database. 
To create/update local database change `ConnectionStrings.DefaultConnection` setting in appsetting.json file to correct server and execute command 'update-database' in PackageManagerConsole or other CLI.  Also functionality can be easily extended to execute migrations on application start (https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/applying?tabs=dotnet-core-cli#apply-migrations-at-runtime).
Update database command can be called as one of deployment's steps or part of container (Dockerfile).
2. Database in memory. Switching to this option is managed by flag UseInMemoryDatabase in appsetting.json file. This features can be used for testing purposes (integration tests).

<b>Authorizaion and Authentification:</b>

Token for testing purpose: eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyLCJ1c2VyaWQiOjU3OTN9.mXLWmKSxcuTX_GM5V6e7WeEDA-dofCsr0ZZldkd7v7k

Current logic just read token and extract userId without token validation and checking that user exists.

<b>Assumptions:</b>
1. Authentification (bearer token) part is implemented as a draft without token and user validation, authorization part (roles, ownership) is fully missed, you can find a few variants of authentification and authorization in my other project https://github.com/tumanina/AuthService. 
2. Some of user input validation was not implemented (email validation, length of name and etc).
3. Only creating order functionality was covered by unit and integration tests. In real application each condition and edge cases must be covered by unit and integration tests.
4. Pagination for get customers and get order was not mentioned, so i missed this, but for real appliation it is good to use page size and page number parameters in api request.
5. Application was implemented as monolithic system. In case of microservice architecture some part of the system can be done other way: for example in case of events based architecture activity logging can be done based on these events, also Saga pattern can be used to solve problem with stock change in order creating functionality.
6. Theoretically application like this can be used as a good example of implementing CQRS, but on my mind for test assesments CQRS is overhead if not specified in requrements.

<b>Stock update problem</b> can also be solved in other ways:
1. Transaction on database level with commit only after all operations done
2. Saga patern with rollback operation in case of microservices implementation
3. Comand patern with Undo operation.
