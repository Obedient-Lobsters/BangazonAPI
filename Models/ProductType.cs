//Author: Leah Gwin
//Purpose: Method for ProductType table

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace BangazonAPI.Models
{
    public class ProductType
    {

        [Key]
        public int ProductTypeId { get; set; }

        [StringLength(30, ErrorMessage = "Maximum length is 30 characters"), Required]
        public string ProductTypeName { get; set; }

    }
}
 