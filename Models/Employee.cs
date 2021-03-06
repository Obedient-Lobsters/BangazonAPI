﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonAPI.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }

        [Required]
        public string FirstName { get; set; }

		[Required]
        public string LastName { get; set; }

		[Required]
		public string Email { get; set; }

		[Required]
		public bool Supervisor { get; set; }

		[Required]
		public int DepartmentId { get; set; }
		
        public Department Department { get; set; }

		public Computer Computer { get; set; }
    }
}
