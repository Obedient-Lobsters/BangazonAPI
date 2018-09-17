//Author: William K. Kimball
//Purpose: Allow a user to communicate with the Bangazon database to GET PUT POST and DELETE entries, and filter certain things via query string paramaters
//Methods: GET PUT POST DELETE

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using BangazonAPI.Models;
using System.Data;

namespace BangazonAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IConfiguration _config;

        public OrderController(IConfiguration config)
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
        // GET order
        // Arguments: 1.)_include controlls logic to select either products or customers. To utilize this query string parameter, input the following url: http://localhost:5000/order?_include=products or http://localhost:5000/order?_include=customers
        // 2.) completed takes either true or false and returns orders that are either completed or not. Example usage: http://localhost:5000/order?completed=true
        [HttpGet]
        public async Task<IActionResult> Get(string _include, string completed)
        {
            using (IDbConnection conn = Connection)
            {
                string sql = "Select * from [Order] JOIN Customer ON [Order].CustomerId = Customer.CustomerId";
                var fullOrder = await conn.QueryAsync<Order>(sql);
                if (_include != null && _include.Contains("products"))
                {

                sql = $" Select * FROM [Order] JOIN ProductOrder ON [Order].OrderId = ProductOrder.OrderId JOIN Product ON ProductOrder.ProductId = Product.ProductId";
                    Dictionary<int, Order> report = new Dictionary<int, Order>();
                    fullOrder = await conn.QueryAsync<Order, Product, Order>(
                    sql, (order, product) =>
                    {
                        // Does the Dictionary already have the key of the OrderId?
                        if (!report.ContainsKey(order.OrderId))
                        {
                            // Create the entry in the dictionary
                            report[order.OrderId] = order;
                        }

                        // Add the product to the current Product entry in Dictionary
                        report[order.OrderId].Product.Add(product);
                        return order;
                    }, splitOn:"OrderId"
                        );
                    return Ok(report.Values);
                }
            if (_include != null && _include.Contains("customers"))
                {
                        sql += $" JOIN Customer as Cust ON [Order].CustomerId = Cust.CustomerId";
                        fullOrder = await conn.QueryAsync<Order, Customer, Order>(
                            sql, (order, c) => { order.Customer = c; return order; }, splitOn: "OrderId, CustomerId");
                    return Ok(fullOrder);
                }

            if (completed != null && completed.Contains("true"))
               {
                        sql = "SELECT * FROM [Order] JOIN Customer ON [Order].CustomerId = Customer.CustomerId";
                        sql += " WHERE [Order].CustomerPaymentId is not NULL";
                     fullOrder = await conn.QueryAsync<Order>(sql);
                    return Ok(fullOrder);
               }
                    if (completed != null && completed.Contains("false"))
                    {
                        sql = "SELECT * FROM [Order] JOIN Customer ON [Order].CustomerId = Customer.CustomerId";
                        sql += " WHERE [Order].CustomerPaymentId is NULL";
                     fullOrder = await conn.QueryAsync<Order>(sql);
                    return Ok(fullOrder);
                }

                    if (completed == null && _include == null)
                {
                    return Ok(fullOrder);
                }
                return Ok();

            };
           

        }

  

        // GET order/5
        [HttpGet("{id}", Name = "GetOrder")]
        //arguments: id specifies which order to get
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            using (IDbConnection conn = Connection)
            {
                string sql = $"SELECT * FROM [Order] WHERE OrderId = {id}";

                var theSingleOrder = (await conn.QueryAsync<Order>(
                    sql)).Single();
                return Ok(theSingleOrder);
            }

        }

        // POST api/order
        [HttpPost]
        //arguments: order specifies the order to be posted. This takes the form of a json object with the value in this format: { "CustomerId":"1"}
    public async Task<IActionResult> Post([FromBody] Order order)
        {
            string sql = $@"INSERT INTO [Order]
            (CustomerId)
            VALUES
            ('{order.CustomerId}');
            select MAX(OrderId) from [Order]";

            using (IDbConnection conn = Connection)
            {
                var newOrderId = (await conn.QueryAsync<int>(sql)).Single();
                order.OrderId = newOrderId;
                return CreatedAtRoute("GetOrder", new { id = newOrderId }, order);
            }

        }

        // PUT api/values/5
        [HttpPut("{id}")]
        //arguments: order specifies the order to be updated. This takes the form of a json object with the value in this format: { "CustomerId":"1", "CustomerPaymentId":"2"}
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] Order order)
        {
            string sql = $@"
            UPDATE [Order]
            SET CustomerPaymentId = '{order.CustomerPaymentId}'
            WHERE OrderId = {id}";

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
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        //This method checks if item exists in database returns a boolean value. This method is refered to in PUT method
        private bool OrderExists(int id)
        {
            string sql = $"SELECT OrderId FROM [Order] WHERE OrderId = {id}";
            using (IDbConnection conn = Connection)
            {
                return conn.Query<PaymentType>(sql).Count() > 0;
            }
        }


        // DELETE order/Delete
        // This method executes the DELETE command. Its parameter is id taken from route
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            string sql = $@"DELETE FROM [Order] WHERE OrderId = {id} IF (OBJECT_ID('dbo.FK_OrderProductOrder', 'F') IS NOT NULL)
                BEGIN
                    ALTER TABLE dbo.ProductOrder DROP CONSTRAINT FK_OrderProductOrder
                END DELETE FROM ProductOrder WHERE OrderId = {id}";
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
    }
}
