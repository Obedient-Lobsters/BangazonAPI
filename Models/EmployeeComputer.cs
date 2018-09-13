using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonAPI.Models
{
    public class EmployeeComputer
    {
        [Key]
        public int EmployeeComputerId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateAssigned { get; set; }

		[DataType(DataType.Date)]
        public DateTime? DateReturned { get; set; }

        [Required]
        public int EmployeeId { get; set; }

		[Required]
        public int ComputerId { get; set; }
    }
}
