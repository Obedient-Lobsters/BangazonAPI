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

        // GET: ProductType/2
        [HttpGet("{id}", Name = "GetProductType")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            using (IDbConnection conn = Connection)
            {
                string sql = $"SELECT * FROM ProductType WHERE productTypeId = {id}";
                var singleProductType = (await conn.QueryAsync<ProductType>(sql)).Single();
                return Ok(singleProductType);
            }
        }


        // POST producttype/POST
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

        // PUT producttype/PUT
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int ProductTypeId, [FromBody] ProductType value)
        {
            string sql = $@"
            UPDATE ProductType
            SET Name = '{value.ProductTypeName}',
            WHERE Id = {ProductTypeId}";

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
                if (!ProductTypeExists(ProductTypeId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }
        //this boolean method checks to see if item already exists in DB, and it is needed for the PUT method
        private bool ProductTypeExists(int id)
        {
            string sql = $"SELECT Id, ProductTypeName FROM ProductType WHERE Id = {id}";
            using (IDbConnection conn = Connection)
            {
                return conn.Query<ProductType>(sql).Count() > 0;
            }
        }


    }
}
