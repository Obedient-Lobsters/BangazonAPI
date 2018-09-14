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

        public PaymentTypeController(IConfiguration config)
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
        // GET: All PaymentType
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
        private bool PaymentTypeExists(int id)
        {
            string sql = $"SELECT PaymentTypeId, PaymentTypeName FROM PaymentType WHERE paymentTypeId = {id}";
            using (IDbConnection conn = Connection)
            {
                return conn.Query<PaymentType>(sql).Count() > 0;
            }
        }


        //// GET: PaymentType/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: PaymentType/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here

        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: PaymentType/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: PaymentType/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}