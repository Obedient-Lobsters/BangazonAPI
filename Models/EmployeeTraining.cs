﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BangazonAPI.Models
{
    public class EmployeeTraining
    {
            [Key]
            public int EmployeeTrainingId { get; set; }

            [Required]
            public int TrainingProgramId { get; set; }

            [Required]
            public int EmployeeId { get; set; }
        }
    }

