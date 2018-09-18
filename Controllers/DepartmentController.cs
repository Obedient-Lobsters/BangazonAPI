using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using BangazonAPI.Models;
using System.Data;

namespace BangazonAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IConfiguration _config;

        public DepartmentController(IConfiguration config)
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
        //   GET /Departments?_include=employees
        [HttpGet]
        public async Task<IActionResult> Get(string _include, string _budget, int _gt)
        {
            using (IDbConnection conn = Connection)
            {
                string sql = "Select DepartmentId, DepartmentName, ExpenseBudget FROM Department";

                if (_include != null && _include.Contains("employee"))
                {

                    sql = $" Select DepartmentId, DepartmentName, ExpenseBudget FROM Department JOIN Employee ON Department.DepartmentId = Employee.DepartmentId";
                    Dictionary<int, Department> report = new Dictionary<int, Department>();
                    var fullDep = await conn.QueryAsync<Department, Employee, Department>(
                    sql, (department, employee) =>
                    {
                        // Does the Dictionary already have the key of the Employee?
                        if (!report.ContainsKey(department.DepartmentId))
                        {
                            // Create the entry in the dictionary
                            report[department.DepartmentId] = department;
                        }

                        // Add the product to the current Product entry in Dictionary
                        report[department.DepartmentId].Employees.Add(employee);
                        return department;
                    }, splitOn: "DepartmentId, EmployeeId"
                        );
                    return Ok(report.Values);
                }

                if (_gt > 1)
                {
                    sql = $@"SELECT DepartmentId, DepartmentName, ExpenseBudget FROM Department WHERE ExpenseBudget >= {_gt}";
                    var budgetDep = await conn.QueryAsync<Department>(
                        sql);
                    return Ok(budgetDep);
                }
                var departments = await conn.QueryAsync<Department>(sql);
                return Ok(departments);
            }
        }
        // GET department/5
        [HttpGet("{id}", Name = "GetDepartment")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            using (IDbConnection conn = Connection)
            {
                string sql = $"SELECT DepartmentId, DepartmentName, ExpenseBudget FROM Department WHERE DepartmentId = {id}";

                var theSingleOrder = (await conn.QueryAsync<Department>(
                    sql)).Single();
                return Ok(theSingleOrder);
            }

        }

        // POST api/department
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Department department)
        {
            string sql = $@"INSERT INTO Department
            (DepartmentName, ExpenseBudget)
            VALUES
            ('{department.DepartmentName}', '{department.ExpenseBudget}');
            select MAX(DepartmentId) from Department";

            using (IDbConnection conn = Connection)
            {
                var newDepartmentId = (await conn.QueryAsync<int>(sql)).Single();
                department.DepartmentId = newDepartmentId;
                return CreatedAtRoute("GetDepartment", new { id = newDepartmentId }, department);
            }

        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] Department department)
        {
            string sql = $@"
            UPDATE Department
            SET DepartmentName = '{department.DepartmentName}',
                ExpenseBudget = '{department.ExpenseBudget}'
            WHERE DepartmentId = {id}";

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
                if (!DepartmentExists(id))
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
        private bool DepartmentExists(int id)
        {
            string sql = $"SELECT DepartmentId FROM Department WHERE Department = {id}";
            using (IDbConnection conn = Connection)
            {
                return conn.Query<PaymentType>(sql).Count() > 0;
            }
        }


        // DELETE order/Delete
        // This method executes the DELETE command. Its parameter is id taken from route
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            string sql = $@"DELETE FROM Department IF (OBJECT_ID('dbo.FK_DepartmentEmployee', 'F') IS NOT NULL)
                                BEGIN
                                  ALTER TABLE dbo.Employee DROP CONSTRAINT FK_DepartmentEmployee
                                END DELETE FROM Department WHERE DepartmentId = {id}";
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

