using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Reports.Models
{
    public class CalculationReport
    {
        public string ShipmentCode { get; set; }
        public string CustomerName { get; set; }
        public string CalculationDate { get; set; }
        public string WeightUom { get; set; }
        public string Currency { get; set; }

        public double? MainFirstElementWeight { get; set; }
        public double? MainSecondElementWeight { get; set; }
        public double? MainThirdElementWeight { get; set; }

        public double? SecondaryFirstElementWeight { get; set; }
        public double? SecondarySecondElementWeight { get; set; }
        public double? SecondaryThirdElementWeight { get; set; }

        public double? BonusFirstElementWeight { get; set; }
        public double? BonusSecondElementWeight { get; set; }
        public double? BonusThirdElementWeight { get; set; }

        public double? PtNetWeight { get; set; }
        public double? PtPrice { get; set; }
        public double? PtResult { get; set; }
        public double? PtPercent { get; set; }
        public double? PtSummary { get; set; }

        public double? PdNetWeight { get; set; }
        public double? PdPrice { get; set; }
        public double? PdResult { get; set; }
        public double? PdPercent { get; set; }
        public double? PdSummary { get; set; }

        public double? RhNetWeight { get; set; }
        public double? RhPrice { get; set; }
        public double? RhResult { get; set; }
        public double? RhPercent { get; set; }
        public double? RhSummary { get; set; }

        public double? CalculationModelElementsSum { get; set; }

        public double? Weight { get; set; }
        public double? Price { get; set; }
        public double? CalculationModelCostsSum { get; set; }

        public double? ResultPercent { get; set; }
        public double? CalculationModelResultSummary { get; set; }

        public double? CalculationModelSummaryPrice { get; set; }

        public double? InvoiceSum { get; set; }
        public double? InvoicePrice { get; set; }
        public double? CalculationModelForInvoiceResult { get; set; }
    }
}