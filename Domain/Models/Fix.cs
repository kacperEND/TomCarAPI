using Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    [Table("Fixs")]
    public class Fix : AuditableModel
    {
        [Required]
        public int? FixOrderId { get; set; }

        public virtual FixOrder FixOrder { get; set; }

        public virtual ICollection<Element> Elements { get; set; }
    }
}