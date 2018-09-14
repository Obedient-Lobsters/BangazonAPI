//Author: Joey Smith
//Purpose: Controller for Products

using System.Data;
using System.Threading.Tasks;
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
        
        public ProductController (IConfiguration config)
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

//        // GET api/values/5
//        [HttpGet("{id}")]
//        public ActionResult<string> Get(int id)
//        {
//            return "value";
//        }

     // POST api/values
       [HttpPost]
        public async Task<IActionResult> Post([FromBody] Product value)
        {
            using (IDbConnection conn = Connection)
            {
                string sql = $@"INSERT INTO Product
                (ProductId, Price, Title, Description, Quantity, CustomerId, ProductTypeId) 
                VALUES
                ();
                select "

            }
        }

//        // PUT api/values/5
//        [HttpPut("{id}")]
//        public void Put(int id, [FromBody] string value)
//        {
//        }

//        // DELETE api/values/5
//        [HttpDelete("{id}")]
//        public void Delete(int id)
//        {
//        }
  }
}
