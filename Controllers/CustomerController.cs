//Author:Shuaib Sajid
//Purpose: Controller for Customer Table
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BangazonAPI.Models;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace BangazonAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IConfiguration _config;

        public CustomerController(IConfiguration config)
        {
            _config = config;
        }

        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

       //This GET method allows user to GET all items in database, Query first and last name with ?q
       //get a list with ?_include=payments and ?_include=products
        [HttpGet]
        public async Task<IActionResult> GET(string q, string _include)
        {
            string sql = "SELECT * FROM Customer ";
            Console.WriteLine(sql);

            using (IDbConnection conn = Connection)
            {
                if (_include == null)
                {
                    _include = "";
                }
                //query name
                if (q != null)
                {
                    sql += $" WHERE 1=1 AND FirstName LIKE '%{q}%' OR LastName LIKE '%{q}%'";
                }
                //include payments
                if (_include != null && _include.Contains("payments"))
                {
                    var fullCustomer = await conn.QueryAsync<Customer>(sql);

                    sql = $@"SELECT *
                           FROM Customer c
                           JOIN CustomerPayment cp ON c.CustomerId = cp.CustomerId
                           JOIN PaymentType p ON cp.PaymentTypeId = p.PaymentTypeId;";
                    Dictionary<int, Customer> customerPayments = new Dictionary<int, Customer>();
                    fullCustomer = await conn.QueryAsync<Customer, PaymentType, Customer>(sql,
                        (customer, paymentType) =>
                        {// Does the Dictionary already have the key of the CustomerId?
                            if (!customerPayments.ContainsKey(customer.CustomerId))
                            {
                                // Create the entry in the dictionary
                                customerPayments[customer.CustomerId] = customer;
                            }

                            // Add the payment to the current PaymentType entry in Dictionary
                            customerPayments[customer.CustomerId].Payments.Add(paymentType);
                            return customer;
                        },
                        splitOn: "CustomerId");
                    return Ok(customerPayments);
                }
                //include products
                if (_include != null && _include.Contains("products"))
                {
                    var fullCustomerProd = await conn.QueryAsync<Customer>(sql);
                    sql = $@"SELECT *
                            FROM Customer c
                            JOIN Product p ON c.CustomerId = p.CustomerId;";
                    Dictionary<int, Customer> customerProduct = new Dictionary<int, Customer>();
                    fullCustomerProd = await conn.QueryAsync<Customer, Product, Customer>(sql,
                        (customer, product) =>
                        {
                            if (!customerProduct.ContainsKey(customer.CustomerId))
                            {
                                // Create the entry in the dictionary
                                customerProduct[customer.CustomerId] = customer;
                            }
                            // Add the product to the current Product entry in Dictionary
                            customerProduct[customer.CustomerId].Products.Add(product);
                            return customer;
                        },
                       splitOn: "CustomerId");
                    return Ok(customerProduct);
                }

                IEnumerable<Customer> customers = await conn.QueryAsync<Customer>(sql);
                return Ok(customers);

            }
        }

                // GET /customers/5
                //This GET method allows user to Get One item, accepts id taken from route
        [HttpGet("{id}", Name = "GetCustomer")]
        public async Task<IActionResult> Get([FromRoute]int id)
        {
            string sql = $"SELECT CustomerId, FirstName FROM Customer WHERE CustomerId = {id}";

            using (IDbConnection conn = Connection)
            {
                IEnumerable<Customer> customers = await conn.QueryAsync<Customer>(sql);
                return Ok(customers);
            }
        }

        // POST /customers
        //This POST methods allows user to post a new item to database. accepts Body on customer type of
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Customer customer)
        {
            string sql = $@"INSERT INTO Customer
                (FirstName
                  ,LastName
                  ,Email
                  ,Address
                  ,City
                  ,State
                  ,AcctCreationDate
                  ,LastLogin)
                VALUES
                ('{customer.FirstName}'
                  ,'{customer.LastName}'
                  ,'{customer.Email}'
                  ,'{customer.Address}'
                  ,'{customer.City}'
                  ,'{customer.State}'
                  ,'{customer.AcctCreationDate}'
                  ,'{customer.LastLogin}');
                select MAX(CustomerId) from Customer;";

            using (IDbConnection conn = Connection)
            {
                var customerId = (await conn.QueryAsync<int>(sql)).Single();
                customer.CustomerId = customerId;
                return CreatedAtRoute("GetCustomer", new { id = customerId }, customer);
            }
        }
        
        //This PUT method allows user to update an item in Database. It accepts id taken from route and Body on customer type of
        [HttpPut("{id}")]
        public async Task<IActionResult> ChangeCustomer(int id, [FromBody] Customer customer)
        {
            string sql = $@"
                UPDATE Customer
                SET FirstName = '{customer.FirstName}'
                  ,LastName = '{customer.LastName}'
                  ,Email = '{customer.Email}'
                  ,Address = '{customer.Address}'
                  ,City = '{customer.City}'
                  ,State = '{customer.State}'
                  ,AcctCreationDate = '{customer.AcctCreationDate}'
                  ,LastLogin = '{customer.LastLogin}'
                WHERE CustomerId = {id}";

            try
            {
                using (IDbConnection conn = Connection)
                {
                    int rowsAffected = await conn.ExecuteAsync(sql);
                    if (rowsAffected > 0)
                    {
                        return new StatusCodeResult(StatusCodes.Status204NoContent);
                    }
                    throw new Exception("No rows affected");
                }
            }
            catch (Exception)
            {
                if (!CustomerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }
        //This method checks database for an existing table based on id
        private bool CustomerExists(int id)
        {
            string sql = $"SELECT CustomerId, FirstName, LastName FROM Customer WHERE CustomerId = {id}";
            using (IDbConnection conn = Connection)
            {
                return conn.Query<Customer>(sql).Count() > 0;
            }
        }
    }
}
    

