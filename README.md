# Inventory Management 

It is a AspNet MVC Core (v 7.0) Project with two Class lIbrary project. Project is designed using Domain Driven Design(DDD) & 3 layer architecture(Presentaion, Service, Data).

## Features

1. Implemented Login & Registration And Authentication with JWT token. Implemented policy based authorization.
2. Admin login (email): admin@gmail.com (password):1234567
3. Clent side and Server side validation is added for every iniput fields in the project.
3. Admin will get all premission for all CRUD operations. Admin will update product sale, purchase order status by clicking edit symbol from table.
4. User can view categories, products, suppliers, create & view own purchase and sale orders, view own transactions when status is approved by admin, view reports own purchase , sale. Can't access inventory report
5. Transaction table will show record when it is approved by admin.
6. Search operations added for every table columns. Multiple field search can be performed together.
7. Uploaded photo is resized in server side.
8. .env file should be added in InventoryManagement.Presentation project. Should see .env.example file to set up environment variables.

## Databse ERD Diagram
[ERD diagram picture link of the project tables](https://drive.google.com/file/d/1I-Hd12vkQFnPCgobqvuCvTohrt2Nk0F5/view?usp=sharing)

[ER Diagram of Database](Modeldatabases01.png)

## Used Technology
1. FrontEnd: Html, Bootstrap, Fontawesome icon, Javascript, JQuery, SweetAlert2
2. Theme: Admin LTE (v 3.2) 
3. DataTable: Tabulator(v 6) 
4. DateTime: Luxon js, FlatPicker
5. Server side: C#, Asp Net MVC Core, Class Library, .Net (v 7), Mapster, Autofac, Serilog(File, Databse) sink
6. ORM: Entity Framework Core
7. Database : SQL SERVER
8. Design: Domain Driven Design, 3 layer Architecture
9. Tool : Visual Studio, Github


## Need to Run the project
 1. Git Clone or download project
 2. Open project in Visual Studio . 
 3. Build project in .Net (v 7)
 4. Use SQL server for Database
 5. Add Database connection string, Jwt key in .env file in InventoryManagement.Presentation project.Given in .env.example .
 6. CONNECTIONSTRINGS__DbCon= "Data Source=.\\SQLEXPRESS;Initial Catalog=;User ID=; Password=;TrustServerCertificate=True;"
 7. Jwt__Key = " " (any string)
 8. Run Migration: dotnet ef database update --project InventoryManagement.Presentation --context ApplicationDbContext
 9. set InventoryManagement.Presentation as startup project.
 8. Build & Run


## Connect with me

linkedin profile[linkedin.com/ahasanur-rahman](https://www.linkedin.com/in/ahasanur-rahman-a10925202/)


# Inventory-Management