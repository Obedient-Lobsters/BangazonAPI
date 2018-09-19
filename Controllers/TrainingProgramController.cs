//Author: Leah Gwin
//Purpose: Controller for ProductType table
//Methods: GET, POST, PUT, DELETE


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

    public class TrainingProgramController : ControllerBase
    {
        private readonly IConfiguration _config;

        public TrainingProgramController(IConfiguration config)
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

        //   GET /TrainingProgram?_include=employees
        //   GET /TrainingProgram?completed=false
        [HttpGet]
        public async Task<IActionResult> Get(string _include, string completed)
        {

            using (IDbConnection conn = Connection)
            {

                string sql = "Select * from TrainingProgram LEFT JOIN EmployeeTraining ON TrainingProgram.TrainingProgramId = EmployeeTraining.EmployeeTrainingId";

                if (_include != null && _include.Contains("employee"))
                {
                    sql = $"Select * FROM TrainingProgram " +
                        $"LEFT JOIN EmployeeTraining ON TrainingProgram.TrainingProgramId = EmployeeTraining.TrainingProgramId " +
                        $"LEFT JOIN Employee ON EmployeeTraining.EmployeeId = Employee.EmployeeId ";

                    Dictionary<int, TrainingProgram> report = new Dictionary<int, TrainingProgram>();
                    var fullTrainingProgram = await conn.QueryAsync<TrainingProgram, Employee, TrainingProgram>(
                    sql, (trainingProgram, employee) =>
                    {
                        // Does the Dictionary already have the key of the Employee?
                        if (!report.ContainsKey(trainingProgram.TrainingProgramId))
                        {
                            // Create the entry in the dictionary
                            report[trainingProgram.TrainingProgramId] = trainingProgram;
                        }

                        // Add the Employees to the current TrainingProgram entry in Dictionary
                        report[trainingProgram.TrainingProgramId].Employees.Add(employee);
                        return trainingProgram;
                    }, splitOn: "TrainingProgramId"
                        );
                    return Ok(report.Values);
                }
                //Checking to see if the Training Program is completed by adding an additional "WHERE" to our sql statement to filter out dates that were in the past from today
                if (completed == "false")
                {
                    DateTime current = DateTime.Today;
                    sql += $" WHERE TrainingProgram.endDate >= '{current}'";
                }   
                var trainingPrograms = await conn.QueryAsync<TrainingProgram>(sql);
                return Ok(trainingPrograms);
            }
        }

        // GET trainingprogram/2
        [HttpGet("{id}", Name = "GetTrainingProgram")]
        //arguments: id specifies which trainingProgram to get
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            using (IDbConnection conn = Connection)
            {
                string sql = $"Select * FROM TrainingProgram " +
                        $"LEFT JOIN EmployeeTraining ON TrainingProgram.TrainingProgramId = EmployeeTraining.TrainingProgramId " +
                        $"LEFT JOIN Employee ON EmployeeTraining.EmployeeId = Employee.EmployeeId " +
                        $"WHERE TrainingProgram.TrainingProgramId = {id}";
                Dictionary<int, TrainingProgram> report = new Dictionary<int, TrainingProgram>();
                var SingleTrainingProgram = (await conn.QueryAsync<TrainingProgram, Employee, TrainingProgram>(
                sql, (TrainingProgram, employee) =>
                {
                    // Does the Dictionary already have the key of the TrainingProgramId?
                    if (!report.ContainsKey(TrainingProgram.TrainingProgramId))
                    {
                        // Create the entry in the dictionary
                        report[TrainingProgram.TrainingProgramId] = TrainingProgram;
                    }

                    // Add the employee to the current Employee entry in Dictionary
                    report[TrainingProgram.TrainingProgramId].Employees.Add(employee);
                    return TrainingProgram;
                }, splitOn: "TrainingProgramId"
                    )).Single();
                return Ok(report.Values);
            }
        }

        // POST /trainingprogram
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TrainingProgram value)
        {
            string sql = $@"INSERT INTO TrainingProgram
            (ProgramName, StartDate, EndDate, MaximumAttendees)
            VALUES
            ('{value.ProgramName}'
            ,'{value.StartDate}'
            ,'{value.EndDate}'
            ,'{value.MaximumAttendees}');
            select MAX(TrainingProgramId) from TrainingProgram";

            using (IDbConnection conn = Connection)
            {
                var newTrainingProgramId = (await conn.QueryAsync<int>(sql)).Single();
                value.TrainingProgramId = newTrainingProgramId;
                return CreatedAtRoute("GetTrainingProgram", new { id = newTrainingProgramId }, value);
            }
        }

        // PUT trainingprogram/5
        [HttpPut("{id}")]
        //arguments: order specifies the order to be updated. This takes the form of a json object with the value in this format: { "CustomerId":"1", "CustomerPaymentId":"2"}
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] TrainingProgram value)
        {
            string sql = $@"
            UPDATE TrainingProgram
            SET 
            ProgramName = '{value.ProgramName}',
            StartDate = '{value.StartDate}',
            EndDate = '{value.EndDate}',
            MaximumAttendees ='{value.MaximumAttendees}'
            WHERE TrainingProgramId = {id}";

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
                if (!TrainingProgramExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        // DELETE trainingprogram/Delete
        //This DELETE method will delete an existing entity for TrainingProgram by ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            //Checking to see if the Training Program can be deleted by adding an additional "WHERE" to our sql statement to filter out dates that were today or in the past
            DateTime current = DateTime.Today;

            string sql = $@"DELETE FROM TrainingProgram 
                            WHERE TrainingProgramId = {id}
                            WHERE TrainingProgram.startDate > '{current}'";
            
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



        private bool TrainingProgramExists(int id)
        {
            string sql = $"SELECT TrainingProgramId FROM TrainingProgram WHERE TrainingProgramId = {id}";
            using (IDbConnection conn = Connection)
            {
                return conn.Query<PaymentType>(sql).Count() > 0;
            }
        }



    }
}
