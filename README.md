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
### 1. Customer
Start the program by cd'ing into the BangazonAPI and using the command `dotnet run`. Once the program is running, open up the Postman desktop app and run the following commands:

#### GET
- select `GET` then paste` localhost:5000/customer` into the field and click send. The result should be an array of all the orders in the database.

- select `GET` then paste `http://localhost:5000/customer?_include=payments` into the field and click send. The result should be an array of all the customers in the database with all of the payment types included in that customers as well.

- select `GET` then paste `http://localhost:5000/customer?_include=products` into the field and click send. The result should be an array of all the customers in the database with all of the products types included in that customers as well.

- select `GET` then paste `http://localhost:5000/customer?q=sat` into the field and click send. The result should be an array of all the customers in the database with first or last names that contains sat.

- select ```GET``` then paste ```localhost:5000/customer/1``` or any other number that showed up in the previous query as CustomerId and click send. The result should be only that object of the specified Customer

#### POST
select ```POST```, then paste ```localhost:5000/customer``` into the field, then click Body underneath the field, then select raw, and then paste this snippet or make one similar 
```
{
        "FirstName": "Test",
        "LastName": "Instructions",
        "Email": "PullRequest@gmail.com",
        "Address": "500 Interstate Drt.",
        "City": "Nashville",
        "State": "TN",
        "AcctCreationDate": "2018-09-18",
        "LastLogin": "2018-09-18"
    }
```
then click send. The result should be the new customer you made.

#### PUT
select `PUT ` then paste` localhost:5000/customer/1` or any other `Customer Id `, then click Body underneath the field, then select raw, and then paste this snippet or make one similar 
```
    {
        "FirstName": "Test",
        "LastName": "Instructions",
        "Email": "PullRequest@gmail.com",
        "Address": "500 Interstate Drt.",
        "City": "Nashville",
        "State": "TN",
        "AcctCreationDate": "2018-09-18",
        "LastLogin": "2018-09-18"
    }
```
You should get nothing back from this. When you run the `GET` query the Customer you specified in your `PUT` query should show the updated, edited information you gave it.

### 3. PaymentType
Use the command ```dotnet run``` to start the program, BangazonAPI. Once the program is running, open up the Postman desktop app and run the following commands for each request method:
 ##### GET
 To GET all product types, select GET in Postman then paste ```localhost:5000/PaymentType``` into the field and click send. The result should be an array of all the payment type in the database that should look like:
 ```
 [
  {
    "paymentTypeId": 1,
    "paymentTypeName": "Visa"
  },
  {
    "paymentTypeId": 2,
    "paymentTypeName": "MasterCard"
  },
  {
    "paymentTypeId": 3,
    "paymentTypeName": "Discover"
  }
 ]
 ```
 To GET a specific, single payment type, add an /{id} to the end of the ```localhost:5000/PaymentType``` URL. The result should only include the single payment type with the Id you added like the below:  
```
[
  {
    "paymentTypeId": 1,
    "paymentTypeName": "Visa"
  }
]
```
 ##### POST
 To POST a new object to your existing array for PaymentType, select POST, then paste ```localhost:5000/PaymentType``` into the field. Then click Body underneath the field, select raw, and then paste this below snippet or make one similar then click send. The result should be the new PaymentType you made:
```
{
	"paymentTypeName": "New Value"
}
```
##### PUT
 To update an existing PaymentType, select PUT then paste ```localhost:5000/paymentType/2``` or any other existing order. Then follow the same directions as the POST example, and change the values then click send: 
```
{
	"paymentTypeName": "New Value"
}
```
You should get nothing back from this besides an OK status. When you run the GET query the computer you specified in your PUT query should show the updated, edited information you gave it.
 
 ##### DELETE
 To DELETE an existing product type, select DELETE then paste ```localhost:5000/PaymentType/2``` or any other existing PaymentType then click send. You should get nothing back from this besides an OK status. When you run the GET query the order with the Id you specified in your DELETE query should no longer exist.

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


#### 5. Product 
Use the command dotnet run to start the program, BangazonAPI. Once the program is running, open up the Postman desktop app and run the following commands for each request method:

##### GET
To GET all products, select GET in Postman then paste localhost:5000/Product into the field and click send. The result should be an array of all the Products in the database that should look like:
```
[
  {
"price": 43.34,
"title": 'details',
"description": 'more details',
"customerId": 1,
"quantity": 3,
"productTypeId": 1 
  },
  {
"price": 66.84,
"title": 'words',
"description": 'english',
"customerId": 2, 
"quantity": 7,
"productTypeId": 2 
  },
  {
"price": 44.44,
"title": 'stuff',
"description": 'and things',
"customerId": 9,
"quantity": 3,
"productTypeId": 3
  }
]
```
To GET a specific, single product, add an /{id} to the end of the localhost:5000/Product URL. The result should only include the single product type with the Id you added like the below:
```
[
  {
"price": 46.11,
"title": 'say',
"description": 'what',
"customerId": 5,
"quantity": 1,
"productTypeId": 5 
  }
]
```
### POST
To POST a new object to your existing array for Product, select POST, then paste localhost:5000/Product into the field. Then click Body underneath the field, select raw, and then paste this below snippet or make one similar then click send. The result should be the new Product you made:
```
 {
"price": 32.23,
"title": 'glad',
"description": 'happy',
"customerId": 8,
"quantity": 48,
"productTypeId": 8
  }
  ```
### PUT
To update an existing product, select PUT then paste localhost:5000/Product/6 or any other existing ProductId. Then follow the same directions as the POST example, and change the values then click send:
```
  {
"price": 22.23,
"title": 'glad',
"description": 'mad',
"customerId": 8,
"quantity": 49,
"productTypeId": 8
  }
```
You should get nothing back from this besides an OK status. When you run the GET query the computer you specified in your PUT query should show the updated, edited information you gave it.

### DELETE
To DELETE an existing product, select DELETE then paste localhost:5000/Product/6 or any other existing ProductId then click send. You should get nothing back from this besides an OK status. When you run the GET query the product with the Id you specified in your DELETE query should no longer exist.



