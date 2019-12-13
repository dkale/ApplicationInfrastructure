> This project will act as the Data Access Layer for the application. It could be used to include the below files:
- EntityFramework DbContext classes - for database which will be queried using EF.
- Repository classes - which will basically inherit from GenericRepository.cs.
  These Repositories will be injected into our service classes for querying entities and CRUD operations.
- EntityConfiguration - The IEntityTypeConfiguration for every Entity mapped to the database table.
- DataAccessors - Classes that will only interact with the database through SQL Queries.
