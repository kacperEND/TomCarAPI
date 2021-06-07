using Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    [Table("FixOrders")]
    public class FixOrder : AuditableModel
    {
        public FixOrder()
        {
            this.Fixs = new List<Fix>();
        }

        public string Number { get; set; }
        public double? NetWeight { get; set; }
        public decimal? IncurredCosts { get; set; }
        public decimal? CurrencyRates { get; set; }

        [Required]
        public int? ShipmentId { get; set; }

        public virtual Shipment Shipment { get; set; }
        public int? CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        public DateTime? OrderDate { get; set; }
        public int? CommonCodeCurrencyId { get; set; }
        public virtual CommonCode CommonCodeCurrency { get; set; }
        public int? CommonCodeWeightUomId { get; set; }
        public virtual CommonCode CommonCodeWeightUom { get; set; }
        public int? CommonCodeStatusId { get; set; }
        public virtual CommonCode CommonCodeStatus { get; set; }
        public virtual ICollection<Fix> Fixs { get; set; }
    }
}