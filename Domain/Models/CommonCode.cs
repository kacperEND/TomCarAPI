using Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    [Table("CommonCodes")]
    public class CommonCode : AuditableModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Code { get; set; }

        public string Description { get; set; }

        public string ExternalReference { get; set; }
        public int? ParentId { get; set; }

        public virtual CommonCode Parent { get; set; }

        public int? SortNumber { get; set; }
    }
}