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
                //if (_include != null)
                //{
                //    if (_include == "product")
                //    {
                //        sql += $" SELECT * FROM ProductOrder JOIN Product ON ProductOrder.ProductId = Product.ProductId";
                //    }
                //    else if (_include == "customers")
                //    {
                //        sql += $" JOIN CustomerPayment ON [Order].CustomerPaymentId = CustomerPayment.CustomerPaymentId ";
                //    }
                //}
                var fullOrder = await conn.QueryAsync<Order>(
                    sql);
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

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
