using Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    [Table("Customers")]
    public class Customer : AuditableModel
    {
        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string Name { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string FirstName { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string SecondName { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string CompanyName { get; set; }

        [Column(TypeName = "nvarchar(15)")]
        public string Phone { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Email { get; set; }

        public int? LocationId { get; set; }

        public virtual Location Location { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string TaxNumber { get; set; }

        public Customer()
        {
        }
    }
}