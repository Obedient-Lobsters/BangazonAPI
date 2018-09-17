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

		// GET /computer
		[HttpGet]
		public async Task<IActionResult> Get()
		{
			string sql = $"SELECT * FROM Employee";

			using (IDbConnection conn = Connection)
			{
				IEnumerable<Employee> employees = await conn.QueryAsync<Employee>(sql);
				return Ok(employees);
			}
		}

		// GET /computer/5
		[HttpGet("{id}", Name = "GetEmployee")]
		public async Task<IActionResult> Get([FromRoute]int id)
		{
			string sql;
			//string sql = $"select e.EmployeeId, e.FirstName, e.LastName, e.Email, e.Supervisor, e.DepartmentId, d.DepartmentName, d.ExpenseBudget, c.ComputerId, c.DatePurchased, c.DateDecommissioned, c.Working, c.ModelName, c.Manufacturer from Employee e join Department d on e.DepartmentId = d.DepartmentId join EmployeeComputer ec on e.EmployeeId = ec.EmployeeId join Computer c on c.ComputerId = ec.ComputerId where e.EmployeeId = {id};";
			sql = "SELECT * FROM Employee JOIN Department ON Employee.DepartmentId = Department.DepartmentId ";
			sql += "JOIN Department as d ON Employee.DepartmentId = d.DepartmentId ";
			sql += $" WHERE Employee.EmployeeId = {id}";
			using (IDbConnection conn = Connection)
			{
				Employee SingleEmployee = (await conn.QueryAsync<Employee, Department, Employee>(sql, (e, d) => { e.Department = d; return e; }, splitOn:"EmployeeId, DepartmentId")).Single();
				return Ok(SingleEmployee);
			}
		}


		// POST /computer
		[HttpPost]
		public async Task<IActionResult> Post([FromBody] Employee employee)
		{

			//DateDecommissioned defaults to a date in the database even when null is passed in
			string sql = $@"INSERT INTO Employee
            (FirstName, LastName, Email, Supervisor, DepartmentId)
            VALUES
            ('{employee.FirstName}', '{employee.LastName}', '{employee.Email}', '{(employee.Supervisor ? 1 : 0)}', '{employee.DepartmentId}');
            select MAX(EmployeeId) from Computer;";

			using (IDbConnection conn = Connection)
			{
				var employeeId = (await conn.QueryAsync<int>(sql)).Single();
				employee.EmployeeId = employeeId;
				return CreatedAtRoute("GetEmployee", new { id = employeeId }, employee);
			}
		}

	}
}
