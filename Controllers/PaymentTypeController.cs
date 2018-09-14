//Author: Shuaib Sajid
//Purpose: Controller for PaymentType table

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
    public class PaymentTypeController : Controller
    {
        private readonly IConfiguration _config;

        // This method... I have no idea how this method works. 
        // I just know that it takes the private _config and gives it the config value 
        // so it can work in the Connection method
        public PaymentTypeController(IConfiguration config)
        {
            _config = config;
        }
        // This method makes the connection to database
        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }
        // GET: All PaymentType
        //This method executes GET command and does require a parameter
        [HttpGet]
        public async Task<IActionResult>Get()
        {
            using (IDbConnection conn = Connection)
            {
                string sql = "SELECT * FROM PaymentType";
                var fullPaymentType = await conn.QueryAsync<PaymentType>(sql);
                return Ok(fullPaymentType);
            }
        }

        // GET: PaymentType/3
        // The Method executes GET one item from database. It's parameter is id taken from route. It is an overload Get method
        [HttpGet("{id}", Name = "GetPaymentType")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            using (IDbConnection conn = Connection)
            {
                string sql = $"SELECT * FROM PaymentType WHERE paymentTypeId = {id}";

                var theSinglePaymentType = (await conn.QueryAsync<PaymentType>(sql)).Single();
                return Ok(theSinglePaymentType);
            }
        }


        // POST: PaymentType/Post
        // This method executes POST command. It's parameter is body from model
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PaymentType paymentType)
        {
            string sql = $@"INSERT INTO PaymentType
            (paymentTypeName)
            VALUES
            ('{paymentType.PaymentTypeName}');
            select MAX(paymentTypeId) from PaymentType;";

            using (IDbConnection conn = Connection)
            {
                var paymentTypeId = (await conn.QueryAsync<int>(sql)).Single();
                paymentType.PaymentTypeId = paymentTypeId;
                return CreatedAtRoute("GetPaymentType", new { id = paymentTypeId }, paymentType);
            }
        }
        // PUT PaymentType/Put
        // This Method excutes the PUT method. It's parameters is id from route, and body from model
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] PaymentType paymentType)
        {
            string sql = $@"
            UPDATE PaymentType
            SET paymentTypeName = '{paymentType.PaymentTypeName}'
            WHERE paymentTypeId = {id}";

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
                if (!PaymentTypeExists(id))
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
        private bool PaymentTypeExists(int id)
        {
            string sql = $"SELECT PaymentTypeId, PaymentTypeName FROM PaymentType WHERE paymentTypeId = {id}";
            using (IDbConnection conn = Connection)
            {
                return conn.Query<PaymentType>(sql).Count() > 0;
            }
        }


        // DELETE PaymentType/Delete
        // This method executes the DELETE command. It parameter is id taken from route
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            string sql = $@"DELETE FROM PaymentType WHERE paymentTypeId = {id}";

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