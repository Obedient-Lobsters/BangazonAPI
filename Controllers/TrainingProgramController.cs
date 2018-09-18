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
                    sql, (TrainingProgram, employee) =>
                    {
                        // Does the Dictionary already have the key of the Employee?
                        if (!report.ContainsKey(TrainingProgram.TrainingProgramId))
                        {
                            // Create the entry in the dictionary
                            report[TrainingProgram.TrainingProgramId] = TrainingProgram;
                        }

                        // Add the Employees to the current TrainingProgram entry in Dictionary
                        report[TrainingProgram.TrainingProgramId].Employees.Add(employee);
                        return TrainingProgram;
                    }, splitOn: "TrainingProgramId"
                        );
                    return Ok(report.Values);
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




    }
}
