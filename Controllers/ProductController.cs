//Author: Joey Smith
//Purpose: Controller for Products

using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Dapper;
using System.Data.SqlClient;
using BangazonAPI.Models;

namespace BangazonAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IConfiguration _config;

        public ProductController(IConfiguration config)
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

        //Get all and Get single methods displayed
        // GET All Products
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            using (IDbConnection conn = Connection)
            {
                string sql = "SELECT * FROM Product";
                var AllProducts = await conn.QueryAsync<Product>(sql);
                return Ok(AllProducts);
            }
        }

        //GET /Product/3
        [HttpGet("{id}", Name = "GetProduct")]
        public async Task<IActionResult> Get([FromRoute]int id)
        {
            using (IDbConnection conn = Connection)

            {
                string sql = $"SELECT * FROM Product WHERE ProductId = {id}";

                var SingleProduct = (await conn.QueryAsync<Product>(sql)).Single();
                return Ok(SingleProduct);
            }


        }

        //This method will demonstrate the POST command
        // POST Product/Post
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Product product)
        {

            string sql = $@"INSERT INTO Product
                 (ProductId, Price, Title, Description, Quantity, CustomerId, ProductTypeId) 
                 VALUES
                ('{product.ProductId}', '{product.Price}', '{product.Title}', '{product.Description}', '{product.Quantity}', '{product.CustomerId}', '{product.ProductTypeId}');
                 select MAX(ProductId) from Product;";

            using (IDbConnection conn = Connection)
            {
                var productId = (await conn.QueryAsync<int>(sql)).Single();
                product.ProductId = productId;
                return CreatedAtRoute("GetProduct", new { id = productId }, product);
            }
        }

        //This method will demostrate the Put function
        // PUT Product/put
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] Product product)
        {
            string sql = $@"
            UPDATE Product
            SET Title = '{product.Title}'
            WHERE productId = {id}";

            try
            {
                using (IDbConnection conn = Connection)
                {
                    int rowsAffected = await conn.ExecuteAsync(sql);
                    if (rowsAffected > 0)
                    {
                        return new StatusCodeResult(StatusCodes.Status204NoContent);
                    }
                    throw new Exception("No rows were affected");
                }

            }
            catch (Exception)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        private bool ProductExists(int id)
        {
            throw new NotImplementedException();
        }

        // DELETE Product/2
        [HttpDelete("{id}")]
                   public async Task<IActionResult> Delete([FromRoute] int id)
        {
            string sql = $@"DELETE FROM Product WHERE ProductId = {id}";

            using (IDbConnection conn = Connection)
            {
                int rowsAffected = await conn.ExecuteAsync(sql);
                if (rowsAffected > 0)
                {
                    return new StatusCodeResult(StatusCodes.Status204NoContent);
                }
                throw new Exception("No Rows Were Affected");
            }
        }
    }
}
