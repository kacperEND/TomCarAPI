using Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;

namespace Domain.Models
{
    [Table("Shipments")]
    public class Shipment : AuditableModel
    {
        [Required]
        public string Code { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Fix> Fixs { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
    }
}