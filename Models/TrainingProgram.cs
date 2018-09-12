using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BangazonAPI.Models
{
    public class TrainingProgram
    {
        [Key]
        public int TrainingProgramId { get; set; }

        [Required]
        public string ProgramName { get; set; }
        [StringLength(80,
            ErrorMessage ="Too Many Characters")]
        public string StartDate { get; set; }
        [StringLength(10,
            ErrorMessage ="Please Enter A Valid Start Date")]
        public string EndDate { get; set; }
        [StringLength(10,
            ErrorMessage = "Please Enter A Valid End Date")]
        public int MaximumAttendees { get; set; }
    }
}
