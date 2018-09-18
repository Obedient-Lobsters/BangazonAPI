# BangazonAPI
Welcome to **Bangazon!** The new virtual marketplace. This marketplace allows customers to buy and sell their products through a single page application web page and its data is tracked through a powerful, hand crafted and solely dedicated API. 
### API Documentation
Documentation can be found in our[Bangazon Wiki](https://github.com/Obedient-Lobsters/BangazonAPI/wiki)
### Table of Contents
(content to be filled)
### Software Requirements
Sql Server Manangment Studio
Visual Studio Community 2017
Google Chrome
### Enitity Relationship Diagram
![ERD](/images/bangazonv2.png)


### Database Setup
In Visual Studio right click on ```BangazonAPI``` and select ```Add -> New Item...```
when the window pops up select ```Data``` underneath ```ASP.NET Core``` and choose ```JSON File``` and name it ```appsettings.json``` then click ```add```
copy the contents of the ```BoilerplateAppSettings.json``` into ```appsettings.json``` then open ```SSMS``` and copy the contents of the ```Server name``` text box and paste where it says ```INSERT_DATABASE_CONNECTION_HERE```
then replace ```INSERT_DATABASE_NAME``` with the name of your database that you've created. 

### Http Request Methods
### 4. Order 
Use the command ```dotnet run``` to start the program, BangazonAPI. Once the program is running, open up the Postman desktop app and run the following commands for each request method:
 ##### GET
 To GET all product types, select GET in Postman then paste ```localhost:5000/order``` into the field and click send. The result should be an array of all the Orders in the database that should look like:
```
[
    {
        "orderId": 1,
        "customerId": 2,
        "customer": null,
        "customerPaymentId": 2,
        "product": []
    },
    {
        "orderId": 2,
        "customerId": 1,
        "customer": null,
        "customerPaymentId": null,
        "product": []
    },
    {
        "orderId": 3,
        "customerId": 3,
        "customer": null,
        "customerPaymentId": 3,
        "product": []
    }
]
```
 To GET a specific, single order, add an /{id} to the end of the ```localhost:5000/order``` URL. The result should only include the single order with the Id you added like the below:  
```
[
    {
        "orderId": 3,
        "customerId": 3,
        "customer": null,
        "customerPaymentId": 3,
        "product": []
    }
]
```
 ##### POST
 To POST a new object to your existing array for Order, select POST, then paste ```localhost:5000/order``` into the field. Then click Body underneath the field, select raw, and then paste this below snippet or make one similar then click send. The result should be the new Order you made:
```
{
"CustomerId":"3"
}
```
 ##### PUT
 To update an existing order, select PUT then paste ```localhost:5000/order/2``` or any other existing order. Then follow the same directions as the POST example, and change the values then click send: 
```
{
"CustomerPaymentId":"NewUpdatedId"
}
```
You should get nothing back from this besides an OK status. When you run the GET query the computer you specified in your PUT query should show the updated, edited information you gave it.
 ##### DELETE
 To DELETE an existing product type, select DELETE then paste ```localhost:5000/order/2``` or any other existing Order then click send. You should get nothing back from this besides an OK status. When you run the GET query the order with the Id you specified in your DELETE query should no longer exist.
###Employees
http methods supported: GET, POST, PUT example body:

### 6. Employee 
Use the command ```dotnet run``` to start the program, BangazonAPI. Once the program is running, open up the Postman desktop app and run the following commands for each request method:
 ##### GET
 To GET all employees, select GET in Postman then paste ```localhost:5000/employee``` into the field and click send. The result should be an array of all the Employees in the database that should look like:
```
[
    {
        "employeeId": 1,
        "firstName": "William",
        "lastName": "Kimball",
        "email": "wkkimball043@gmail.com",
        "supervisor": true,
        "departmentId": 1,
        "department": {
            "departmentId": 1,
            "departmentName": "CodeRockstars",
            "expenseBudget": 140234,
            "employees": []
        },
        "computer": null
    },
    {
        "employeeId": 2,
        "firstName": "Robert",
        "lastName": "Leedy",
        "email": "rleedy@gmail.com",
        "supervisor": false,
        "departmentId": 2,
        "department": {
            "departmentId": 2,
            "departmentName": "IT",
            "expenseBudget": 23400,
            "employees": []
        },
        "computer": {
            "computerId": 1,
            "datePurchased": "2017-10-11T00:00:00",
            "dateDecommissioned": null,
            "working": true,
            "modelName": "XPS",
            "manufacturer": "Dell"
        }
    },
    {
        "employeeId": 3,
        "firstName": "Seth",
        "lastName": "Dana",
        "email": "sd@gmail.com",
        "supervisor": false,
        "departmentId": 3,
        "department": {
            "departmentId": 3,
            "departmentName": "Sales",
            "expenseBudget": 24000,
            "employees": []
        },
        "computer": {
            "computerId": 3,
            "datePurchased": "2018-12-11T00:00:00",
            "dateDecommissioned": null,
            "working": true,
            "modelName": "Pro",
            "manufacturer": "Mac"
        }
    }
]
```
 To GET a specific, single order, add an /{id} to the end of the ```localhost:5000/employee``` URL. The result should only include the single employee with the Id you added like the below:  
```
{
    "employeeId": 1,
    "firstName": "William",
    "lastName": "Kimball",
    "email": "wkkimball043@gmail.com",
    "supervisor": true,
    "departmentId": 1,
    "department": {
        "departmentId": 1,
        "departmentName": "CodeRockstars",
        "expenseBudget": 140234,
        "employees": []
    },
    "computer": null
}
```
 ##### POST
 To POST a new object to your existing array for Employee, select POST, then paste ```localhost:5000/employee``` into the field. Then click Body underneath the field, select raw, and then paste this below snippet or make one similar then click send. The result should be the new Employee you made:
```
{
	"FirstName": "Example",
	"LastName": "Person",
	"Email": "fakeemail@thingy.com",
	"Supervisor": true,
	"DepartmentId": 2
}
```
 ##### PUT
 To update an existing employee, select PUT then paste ```localhost:5000/employee/2``` or any other existing employee. Then follow the same directions as the POST example, and change the values then click send: 
```
{
"employeeId": 2,
    "firstName": "Jack",
    "lastName": "Bob",
    "email": "jackbob@gmail.com",
    "supervisor": false,
    "departmentId": 2,
}
```
You should get nothing back from this besides an OK status. When you run the GET query the employee you specified in your PUT query should show the updated, edited information you gave it.
