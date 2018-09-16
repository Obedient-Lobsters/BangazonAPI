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
    [Route("api/[controller]")]
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
        [HttpGet]
        public async Task<IActionResult> Get(string _include)
        {
            using (IDbConnection conn = Connection)
            {
                string sql = "SELECT * FROM [Order] JOIN Customer ON [Order].CustomerId = Customer.CustomerId JOIN CustomerPayment ON [Order].CustomerPaymentId = CustomerPayment.CustomerPaymentId";
                var fullOrder = await conn.QueryAsync<Order>(sql);
                if (_include != null)
                {
                    if (_include == "products")
                    {
                        sql += $"SELECT Price, Title, Description FROM ProductOrder JOIN Product ON ProductOrder.ProductId = Product.ProductId ";
                        fullOrder = await conn.QueryAsync<Order, List<Product>, Order>(
                            sql, (b, a) => { b.Product = a; return b; });
                    }
                    else if (_include == "customers")
                    {
                        sql += $"JOIN Customer as Cust ON [Order].CustomerId = Cust.CustomerId";
                        fullOrder = await conn.QueryAsync<Order, Customer, Order>(
                            sql, (b, c) => { b.Customer = c; return b; }, splitOn: "OrderId, CustomerId");
                    }
                }
                return Ok(fullOrder);
            }
        }

        // GET order/5
        [HttpGet("{id}")]
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
                return CreatedAtRoute("GetOrder", new { OrderId = newOrderId }, order);
            }

        }

        // PUT api/values/5
        [HttpPut("{id}")]
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
