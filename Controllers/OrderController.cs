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
        // GET api/order
        [HttpGet]
        public async Task<IActionResult> Get(string argument)
        {
            using (IDbConnection conn = Connection)
            {
                string sql = "SELECT * FROM [Order] JOIN Customer ON [Order].CustomerId = Customer.CustomerId JOIN CustomerPayment ON [Order].CustomerPaymentId = CustomerPayment.CustomerPaymentId";
                if (argument != null)
                {

                }
                var fullOrder = await conn.QueryAsync<Order>(
                    sql);
                return Ok(fullOrder);
            }
        }

        // GET api/order/5
        [HttpGet("{id}")]
        async Task<IActionResult> Get([FromRoute] int id)
        {
            using (IDbConnection conn = Connection)
            {
                string sql = $"SELECT * FROM Order WHERE OrderId = {id}";

                var fullOrder = await conn.QueryAsync<Order>(
                    sql);
                return Ok(fullOrder);
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
