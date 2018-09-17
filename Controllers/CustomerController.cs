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

        /*
            GET /customers?q=test
            GET /customers?_include=payments
         */
        [HttpGet]
        public async Task<IActionResult> GET(string q, string _include)
        {
            string sql = "SELECT * FROM Customer ";
            if(_include == null)
            {
                _include = "";
            }

            if (q != null)
            {
                sql += $" WHERE 1-1 AND FirstName LIKE '%{q}%' OR LastName LIKE '%{q}%'";
            }
            Console.WriteLine(sql);

            using (IDbConnection conn = Connection)
            {

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

                            // Add the product to the current Product entry in Dictionary
                            customerPayments[customer.CustomerId].Payments.Add(paymentType);
                            return customer;
                        },
                        splitOn: "CustomerId");
                    return Ok(customerPayments);
                }
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
                //    IEnumerable<Customer> customers = await conn.QueryAsync<Customer>(sql);
                //    return Ok(customers);
                //}
                //}

                // GET /customers/5
                //    [HttpGet("{id}", Name = "GetCustomer")]
                //    public async Task<IActionResult> Get([FromRoute]int id)
                //    {
                //        string sql = $"SELECT Id, Name, Language FROM Customer WHERE Id = {id}";

                //        using (IDbConnection conn = Connection)
                //        {
                //            IEnumerable<Customer> customers = await conn.QueryAsync<Customer>(sql);
                //            return Ok(customers);
                //        }
                //    }

                //    // POST /customers
                //    [HttpPost]
                //    public async Task<IActionResult> Post([FromBody] Customer customer)
                //    {
                //        string sql = $@"INSERT INTO Customer
                //        ()
                //        VALUES
                //        ();
                //        select MAX(Id) from Customer;";

                //        using (IDbConnection conn = Connection)
                //        {
                //            var customerId = (await conn.QueryAsync<int>(sql)).Single();
                //            customer.Id = customerId;
                //            return CreatedAtRoute("GetCustomer", new { id = customerId }, customer);
                //        }
                //    }

                //    /*
                //        PUT /customers/5
                //        The [HttpPut] attribute ensures that this method will handle any
                //        request to a `/customers/{id}` with the PUT HTTP verb. Alternatively,
                //        I could name this method `PutCustomer`, or just `Put` and ASP.NET
                //        will detect that the word `Put` is in the method name and ensure
                //        that it will only be invoke for PUT operations.
                //        All other controllers have this method named as `Put`. It's named
                //        differently here to show that the [HttpPut] attribute enforces which
                //        verb is handled.
                //     */
                //    [HttpPut("{id}")]
                //    public async Task<IActionResult> ChangeCustomer(int id, [FromBody] Customer customer)
                //    {
                //        string sql = $@"
                //        UPDATE Customer
                //        SET '
                //        WHERE Id = {id}";

                //        try
                //        {
                //            using (IDbConnection conn = Connection)
                //            {
                //                int rowsAffected = await conn.ExecuteAsync(sql);
                //                if (rowsAffected > 0)
                //                {
                //                    return new StatusCodeResult(StatusCodes.Status204NoContent);
                //                }
                //                throw new Exception("No rows affected");
                //            }
                //        }
                //        catch (Exception)
                //        {
                //            if (!CustomerExists(id))
                //            {
                //                return NotFound();
                //            }
                //            else
                //            {
                //                throw;
                //            }
                //        }
                //    }

                //    // DELETE /customers/5
                //    [HttpDelete("{id}")]
                //    public async Task<IActionResult> Delete(int id)
                //    {
                //        string sql = $@"DELETE FROM Customer WHERE Id = {id}";

                //        using (IDbConnection conn = Connection)
                //        {
                //            int rowsAffected = await conn.ExecuteAsync(sql);
                //            if (rowsAffected > 0)
                //            {
                //                return new StatusCodeResult(StatusCodes.Status204NoContent);
                //            }
                //            throw new Exception("No rows affected");
                //        }

                //    }

                //    private bool CustomerExists(int id)
                //    {
                //        string sql = $"SELECT Id, Name, Language FROM Customer WHERE Id = {id}";
                //        using (IDbConnection conn = Connection)
                //        {
                //            return conn.Query<Customer>(sql).Count() > 0;
                //        }
                //    }
                //}
            }
    }
}
