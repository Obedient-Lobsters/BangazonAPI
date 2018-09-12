using System.ComponentModel.DataAnnotations;


namespace BangazonAPI.Models
{   
    public class PaymentType
    {
        [Key]
        public int PaymentTypeId { get; set; }
        [StringLength(15, ErrorMessage = "Maximum length is 15 characters"),Required]
        public string PaymentTypeName { get; set; }
    }
}
