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
        public async Task<IActionResult> Get(string _include)
        {
            using (IDbConnection conn = Connection)
            {
                string sql = "Select * FROM TrainingProgram";

                if (_include != null && _include.Contains("employee"))
                {

                    sql = $" Select * FROM TrainingProgram JOIN Employee ON TrainingProgram.TrainingProgramId = Employee.TrainingProgramId";
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
                    }, splitOn: "TrainingProgramId, EmployeeId"
                        );
                    return Ok(report.Values);
                }

                
                var trainingPrograms = await conn.QueryAsync<TrainingProgram>(sql);
                return Ok(trainingPrograms);
            }
        }





    }
}
