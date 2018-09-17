using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using BangazonAPI.Models;
using Microsoft.AspNetCore.Http;

namespace BangazonAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]

    public class ProductTypeController : ControllerBase
    {
        private readonly IConfiguration _config;

        public ProductTypeController(IConfiguration config)
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

        // GET /producttype
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            using (IDbConnection conn = Connection)
            {
                string sql = "SELECT * FROM ProductType";

                var fullProductType = await conn.QueryAsync<ProductType>(
                    sql);
                return Ok(fullProductType);
            }
        }


        // POST /values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProductType value)
        {
            string sql = $@"INSERT INTO ProductType
            (ProductTypeName)
            VALUES
            ('{value.ProductTypeName}');
            select MAX(ProductTypeId) from ProductType";

            using (IDbConnection conn = Connection)
            {
                var newProductTypeId = (await conn.QueryAsync<int>(sql)).Single();
                value.ProductTypeId = newProductTypeId;
                return CreatedAtRoute("GetProductType", new { id = newProductTypeId }, value);
            }

        }


    }
}
