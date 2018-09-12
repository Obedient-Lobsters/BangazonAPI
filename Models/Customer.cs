using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BangazonAPI.Models
{

    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        [StringLength(30, ErrorMessage = "Max Character limit is 30 characters"),Required]
        public string FirstName { get; set;}
        [StringLength(30, ErrorMessage = "Max Character limit is 30 characters"),Required]
        public string LastName { get; set; }
        [StringLength(50, ErrorMessage = "Max Character limit is 50 characters"),Required]
        public string Email { get; set; }
        [StringLength(80, ErrorMessage = "Max Character limit is 80 characters"),Required]
        public string Address { get; set; }
        [StringLength(30, ErrorMessage = "Max Character limit is 30 characters"),Required]
        public string City { get; set; }
        [StringLength(2, ErrorMessage = "Max Character limit is 2 characters"),Required]
        public string State { get; set; }

        [DataType(DataType.Date),Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime AcctCreationDate { get; set; }
        //Creating DateTime on account creation for property LastLogin is acceptable per Senior Dev Jordan
        //This property is currently not tracked or modified in issues or tickets
        [DataType(DataType.Date),Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime LastLogin { get; set; }

        public IEnumerable<Product> Products;
        public IEnumerable<PaymentType> Payments;
        public IEnumerable<Customer> Customers;
    }
}
