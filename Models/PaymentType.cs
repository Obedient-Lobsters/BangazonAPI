using System.ComponentModel.DataAnnotations;


namespace BangazonAPI.Models
{   
    //    Structure in Database
    //    CREATE TABLE PaymentType(
    //    PaymentTypeId INTEGER NOT NULL PRIMARY KEY,
    //    PaymentTypeName varchar(15) NOT NULL
    //);
    public class PaymentType
    {
        [Key]
        public int PaymentTypeId { get; set; }
        [StringLength(15, ErrorMessage = "Maxinum lenght is 15 characters"),Required]
        public string PaymentTypeName { get; set; }
    }
}
