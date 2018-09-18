# BangazonAPI
Welcome to **Bangazon!** The new virtual marketplace. This marketplace allows customers to buy and sell their products through a single page application web page and its data is tracked through a powerful, hand crafted and solely dedicated API. 
## API Documentation
Documentation can be found in our [Bangazon Wiki](https://github.com/Obedient-Lobsters/BangazonAPI/wiki)
## Table of Contents
(content to be filled)
## Software Requirements
Sql Server Manangment Studio
Visual Studio Community 2017
Google Chrome
## Enitity Relationship Diagram
![ERD](/images/bangazonv2.png)


## Database Setup
In Visual Studio right click on ```BangazonAPI``` and select ```Add -> New Item...```
when the window pops up select ```Data``` underneath ```ASP.NET Core``` and choose ```JSON File``` and name it ```appsettings.json``` then click ```add```
copy the contents of the ```BoilerplateAppSettings.json``` into ```appsettings.json``` then open ```SSMS``` and copy the contents of the ```Server name``` text box and paste where it says ```INSERT_DATABASE_CONNECTION_HERE```
then replace ```INSERT_DATABASE_NAME``` with the name of your database that you've created. 



### Http Request Methods

### 4. Order 
Use the command ```dotnet run``` to start the program, BangazonAPI. Once the program is running, open up the Postman desktop app and run the following commands for each request method:
 ##### GET
 To GET all Orders, select GET in Postman then paste ```localhost:5000/order``` into the field and click send. The result should be an array of all the Orders in the database that should look like:
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
You should get nothing back from this besides an OK status. When you run the GET query the Order you specified in your PUT query should show the updated, edited information you gave it.
 ##### DELETE

 To DELETE an existing Order, select DELETE then paste ```localhost:5000/order/2``` or any other existing Order then click send. You should get nothing back from this besides an OK status. When you run the GET query the order with the Id you specified in your DELETE query should no longer exist.

### 7. Department 
Use the command ```dotnet run``` to start the program, BangazonAPI. Once the program is running, open up the Postman desktop app and run the following commands for each request method:
 ##### GET
 To GET all Departments, select GET in Postman then paste ```localhost:5000/department``` into the field and click send. The result should be an array of all the Departments in the database that should look like:
```
[
    {
        "departmentId": 1,
        "departmentName": "CodeRockstars",
        "expenseBudget": 140234,
        "employees": []
    },
    {
        "departmentId": 2,
        "departmentName": "IT",
        "expenseBudget": 23400,
        "employees": []
    },
    {
        "departmentId": 3,
        "departmentName": "Sales",
        "expenseBudget": 24000,
        "employees": []
    }
]
```

- select `GET` then paste `http://localhost:5000/department?_include=employee` into the field and click send. The result should be an array of all the department in the database with all of the products included in that department as well.
```
[            
{
 "employeeId": 1,
 "firstName": "William",
 "lastName": "Kimball",
 "email": "wkkimball043@gmail.com",
 "supervisor": true,
 "departmentId": 1,
 "department": null,
 "computer": null
}
]
```
 To GET a specific, single Department, add an /{id} to the end of the ```localhost:5000/department``` URL. The result should only include the single department with the Id you added like the below:  
```
[
    {
        "departmentId": 3,
        "departmentName": "Sales",
        "expenseBudget": 24000,
        "employees": []
    }
]
```
 ##### POST
 To POST a new object to your existing array for Department, select POST, then paste ```localhost:5000/department``` into the field. Then click Body underneath the field, select raw, and then paste this below snippet or make one similar then click send. The result should be the new Department you made:
```
    {
        "DepartmentName": "Test",
        "ExpenseBudget": "300000"
    }
```
 ##### PUT
 To update an existing Department, select PUT then paste ```localhost:5000/Department/2``` or any other existing department. Then follow the same directions as the POST example, and change the values then click send: 
```
{
"DepartmentName":"NewDepartmentName",
"DepartmentName":"234234"
}
```
You should get nothing back from this besides an OK status. When you run the GET query the Department you specified in your PUT query should show the updated, edited information you gave it.
 ##### DELETE
 To DELETE an existing Department, select DELETE then paste ```localhost:5000/department/2``` or any other existing Department then click send. You should get nothing back from this besides an OK status. When you run the GET query the order with the Id you specified in your DELETE query should no longer exist.

###Employees
http methods supported: GET, POST, PUT example body:

 To DELETE an existing product type, select DELETE then paste ```localhost:5000/order/2``` or any other existing Order then click send. You should get nothing back from this besides an OK status. When you run the GET query the order with the Id you specified in your DELETE query should no longer exist.


### 5. Product Type
Use the command ```dotnet run``` to start the program, BangazonAPI. Once the program is running, open up the Postman desktop app and run the following commands for each request method:

##### GET

To GET all product types, select GET in Postman then paste ```localhost:5000/producttype``` into the field and click send. The result should be an array of all the ProductTypes in the database that should look like:
```
[
  {
    "productTypeId": 1,
    "productTypeName": "KnitCap"
  },
  {
    "productTypeId": 2,
    "productTypeName": "Craft Thing"
  },
  {
    "productTypeId": 3,
    "productTypeName": "Poem"
  }
]
```

To GET a specific, single product type, add an /{id} to the end of the ```localhost:5000/producttype``` URL. The result should only include the single product type with the Id you added like the below:  
```
[
  {
    "productTypeId": 1,
    "productTypeName": "KnitCap"
  }
]
```

##### POST

To POST a new object to your existing array for Product Type, select POST, then paste ```localhost:5000/producttype``` into the field. Then click Body underneath the field, select raw, and then paste this below snippet or make one similar then click send. The result should be the new ProductType you made:
```
{
"productTypeName":"Appliances"
}
```

##### PUT

To update an existing product type, select PUT then paste ```localhost:5000/producttype/6``` or any other existing ProductTypeId. Then follow the same directions as the POST example, and change the values then click send: 
```
{
"productTypeName":"NewUpdatedName"
}
```
You should get nothing back from this besides an OK status. When you run the GET query the computer you specified in your PUT query should show the updated, edited information you gave it.

##### DELETE

To DELETE an existing product type, select DELETE then paste ```localhost:5000/producttype/6``` or any other existing ProductTypeId then click send. You should get nothing back from this besides an OK status. When you run the GET query the product type with the Id you specified in your DELETE query should no longer exist.

