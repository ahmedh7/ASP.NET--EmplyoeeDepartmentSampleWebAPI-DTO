# ASP.NET-WebAPI-DTO
WebAPI using ASP.NET and C#, Implementing DTO (Data Transfer Object) Design Pattern
The solution can be downloaded, then connected to a database then run correct

### BL: 
- The layer that handles the business logic
### DAL :
- The layer tha handles Database access

#### Usage
- Access the API trhough your fav tool (e.g. Postman) or through the solution integrated swagger app.
- The API has 2 login endpoints: Staticlogin (shouldn't be used) which uses saved username and password (Hard coded), and login which connects to database.
- A user with "Employee" role may only update and view depaartment info.
- A user with "Manager" role may access al department data and delete departments.
- A user can be created through 2 registration endpoints, "RegisterManager" and "RegisterEmployee", and gaining a role accordingly.

