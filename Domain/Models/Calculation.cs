using Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    [Table("Calculations")]
    public class Calculation : AuditableModel
    {
        public int? PtId { get; set; }
        public DateTime? CalculationDate { get; set; }
        public virtual Element Pt { get; set; }
        public int? PdId { get; set; }
        public virtual Element Pd { get; set; }
        public int? RhId { get; set; }
        public virtual Element Rh { get; set; }
        public int? MainFirstElementId { get; set; }
        public virtual Element MainFirstElement { get; set; }
        public int? MainSecondElementId { get; set; }
        public virtual Element MainSecondElement { get; set; }
        public int? MainThirdElementId { get; set; }
        public virtual Element MainThirdElement { get; set; }
        public int? SecondaryFirstElementId { get; set; }
        public virtual Element SecondaryFirstElement { get; set; }
        public int? SecondarySecondElementId { get; set; }
        public virtual Element SecondarySecondElement { get; set; }
        public int? SecondaryThirdElementId { get; set; }
        public virtual Element SecondaryThirdElement { get; set; }
        public int? BonusFirstElementId { get; set; }
        public virtual Element BonusFirstElement { get; set; }
        public int? BonusSecondElementId { get; set; }
        public virtual Element BonusSecondElement { get; set; }
        public int? BonusThirdElementId { get; set; }
        public virtual Element BonusThirdElement { get; set; }
        public double? Weight { get; set; }
        public double? Price { get; set; }
        public double? ResultPercent { get; set; }
        public double? InvoiceSum { get; set; }
        public double? InvoicePrice { get; set; }
    }
}