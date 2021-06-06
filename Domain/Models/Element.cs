using Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    [Table("Elements")]
    public class Element : AuditableModel
    {
        [Required]
        public int? FixId { get; set; }

        public virtual Fix Fix { get; set; }

        public int? CommonCodeNameId { get; set; }

        public virtual CommonCode CommonCodeName { get; set; }

        public decimal? Price { get; set; }

        public double? NetWeight { get; set; }
    }
}