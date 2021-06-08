using Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    [Table("AppConfig")]
    public class AppConfig : AuditableModel
    {
        public string Key { get; set; }

        [StringLength(int.MaxValue)]
        public string Value { get; set; }
    }
}