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
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            using (IDbConnection conn = Connection)
            {
                string sql = $"SELECT * FROM PaymentType WHERE paymentTypeId = {id}";

                var theSinglePaymentType = (await conn.QueryAsync<PaymentType>(sql)).Single();
                return Ok(theSinglePaymentType);
            }
        }

        //// GET: PaymentType/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: PaymentType/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here

        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

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