//Author: Jordan W. Williams
//Purpose: Allow a user to communicate with the Bangazon database to GET PUT POST and DELETE entries.
//Methods: GET PUT(id) POST DELETE(id)



using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using BangazonAPI.Models;

namespace BangazonAPI.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class ComputerController : ControllerBase
	{
		private readonly IConfiguration _config;

		public ComputerController(IConfiguration config)
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

		// GET /computer
		[HttpGet]
		public async Task<IActionResult> Get()
		{
			string sql = $"SELECT * FROM Computer";

			using (IDbConnection conn = Connection)
			{
				IEnumerable<Computer> computers = await conn.QueryAsync<Computer>(sql);
				return Ok(computers);
			}
		}



		// GET /computer/5
		[HttpGet("{id}", Name = "GetComputer")]
		public async Task<IActionResult> Get([FromRoute]int id)
		{
			string sql = $"SELECT * FROM Computer WHERE ComputerId = {id}";

			using (IDbConnection conn = Connection)
			{
				Computer SingleComputer = (await conn.QueryAsync<Computer>(sql)).Single();
				return Ok(SingleComputer);
			}
		}


		// POST /computer
		[HttpPost]
		public async Task<IActionResult> Post([FromBody] Computer computer)
		{

			string sql = $@"INSERT INTO Computer
            (DatePurchased, DateDecommissioned, Working, ModelName, Manufacturer)
            VALUES
            ('{computer.DatePurchased}', {(computer.DateDecommissioned == null ? "null" : $"'{computer.DateDecommissioned }'")}, '{(computer.Working ? 1 : 0)}', '{computer.ModelName}', '{computer.Manufacturer}');
            select MAX(ComputerId) from Computer;";

			using (IDbConnection conn = Connection)
			{
				var computerId = (await conn.QueryAsync<int>(sql)).Single();
				computer.ComputerId = computerId;
				return CreatedAtRoute("GetComputer", new { id = computerId }, computer);
			}
		}

		// PUT /computer/5
		[HttpPut("{id}")]
		public async Task<IActionResult> ChangeComputer([FromRoute]int id, [FromBody] Computer computer)
		{
			string sql = $@"
            UPDATE Computer
            SET DatePurchased = '{computer.DatePurchased}',
				DateDecommissioned = '{computer.DateDecommissioned}',
				Working = '{(computer.Working ? 1 : 0)}',
				ModelName = '{computer.ModelName}',
				Manufacturer = '{computer.Manufacturer}'
            WHERE ComputerId = {id}";

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
				if (!ComputerExists(id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}
		}

		// DELETE computer/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete([FromRoute] int id)
		{
			string sql = $@"DELETE FROM Computer WHERE ComputerId = {id}";

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

		private bool ComputerExists(int id)
		{
			string sql = $"SELECT * FROM Computer WHERE ComputerId = {id}";
			using (IDbConnection conn = Connection)
			{
				return conn.Query<Computer>(sql).Count() > 0;
			}
		}

	}
}