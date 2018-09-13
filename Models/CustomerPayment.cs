using System.ComponentModel.DataAnnotations;


namespace BangazonAPI.Models
{
    public class CustomerPayment
    {
        [Key]
        public int CustomerPaymentId { get; set; }

        [Required]
        public int CardNumber { get; set; }

        [StringLength(3,ErrorMessage = "CCV code is three numbers"),Required]
        public string CcvCode { get; set; }

        [StringLength(10, MinimumLength = 4,ErrorMessage = "Expiration Date is four to ten numbers"),Required]
        public string ExpirationDate { get; set;}
        //Foreign Keys
        [Required]
        public int CustomerId { get; set; }
        [Required]
        public int PaymentTypeId { get; set; }

    }
}
