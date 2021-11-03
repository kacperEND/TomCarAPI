using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.MongoDB
{
    public class Shipment : CoreAuditableMongoModel
    {
        public int? ShipmentNo { get; set; }
        public string Date { get; set; }
        public string CompanyName { get; set; }

        public FixLite FixLite { get; set; }
        public Revision Revision { get; set; }
    }
}