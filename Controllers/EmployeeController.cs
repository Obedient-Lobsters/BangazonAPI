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
	public class EmployeeController : ControllerBase
	{
		private readonly IConfiguration _config;

		public EmployeeController(IConfiguration config)
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

		// GET /employee
		[HttpGet]
		public async Task<IActionResult> Get()
		{
			string sql;
			sql = "SELECT * FROM Employee LEFT JOIN Department ON Employee.DepartmentId = Department.DepartmentId ";
			sql += "LEFT JOIN Department as d ON Employee.DepartmentId = d.DepartmentId ";
			sql += "LEFT JOIN EmployeeComputer as ec ON Employee.EmployeeId = ec.EmployeeId ";
			sql += "LEFT JOIN Computer as c ON ec.ComputerId = c.ComputerId ";
			using (IDbConnection conn = Connection)
			{
				IEnumerable<Employee> employees = await conn.QueryAsync<Employee, Department, Computer, Employee>(sql, (e, d, c) => { e.Department = d; e.Computer = c; return e; }, splitOn:"EmployeeId, DepartmentId, ComputerId");
				return Ok(employees);
			}
		}

		// GET /employee/5
		[HttpGet("{id}", Name = "GetEmployee")]
		public async Task<IActionResult> Get([FromRoute]int id)
		{
			string sql;
			sql = "SELECT * FROM Employee LEFT JOIN Department ON Employee.DepartmentId = Department.DepartmentId ";
			sql += "LEFT JOIN Department as d ON Employee.DepartmentId = d.DepartmentId ";
			sql += "LEFT JOIN EmployeeComputer as ec ON Employee.EmployeeId = ec.EmployeeId ";
			sql += "LEFT JOIN Computer as c ON ec.ComputerId = c.ComputerId ";
			sql += $" WHERE Employee.EmployeeId = {id}";
			using (IDbConnection conn = Connection)
			{
				Employee SingleEmployee = (await conn.QueryAsync<Employee, Department, Computer, Employee>(sql, (e, d, c) => { e.Department = d; e.Computer = c; return e; }, splitOn:"EmployeeId, DepartmentId, ComputerId")).Single();
				return Ok(SingleEmployee);
			}
		}


		// POST /employee
		[HttpPost]
		public async Task<IActionResult> Post([FromBody] Employee employee)
		{
			
			string sql = $@"INSERT INTO Employee
            (FirstName, LastName, Email, Supervisor, DepartmentId)
            VALUES
            ('{employee.FirstName}', '{employee.LastName}', '{employee.Email}', '{(employee.Supervisor ? 1 : 0)}', '{employee.DepartmentId}');
            select MAX(EmployeeId) from Employee;";

			using (IDbConnection conn = Connection)
			{
				var employeeId = (await conn.QueryAsync<int>(sql)).Single();
				employee.EmployeeId = employeeId;
				return CreatedAtRoute("GetEmployee", new { id = employeeId }, employee);
			}
		}


		// PUT /employee/5
		[HttpPut("{id}")]
		public async Task<IActionResult> ChangeEmployee([FromRoute]int id, [FromBody] Employee employee)
		{
			string sql = $@"
            UPDATE Employee
            SET FirstName = '{employee.FirstName}',
				LastName = '{employee.LastName}',
				Email = '{employee.Email}',
				Supervisor = '{employee.Supervisor}',
				DepartmentId = '{employee.DepartmentId}'
            WHERE EmployeeId = {id}";

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
				if (!EmployeeExists(id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}
		}

		private bool EmployeeExists(int id)
		{
			string sql = $"SELECT * FROM Employee WHERE EmployeeId = {id}";
			using (IDbConnection conn = Connection)
			{
				return conn.Query<Employee>(sql).Count() > 0;
			}
		}

	}
}
