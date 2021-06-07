using System.Collections.Generic;

namespace Application.Reports.Models
{
    public class FixOrderReport
    {
        public FixOrderReport()
        {
            Fixs = new List<FixReport>();
        }

        public string ShipmentCode { get; set; }
        public string OrderDate { get; set; }
        public string WeightUom { get; set; }
        public string Currency { get; set; }
        public double SumFixs { get; set; }
        public double NetWeight { get; set; }
        public decimal IncurredCosts { get; set; }
        public double Cost { get; set; }
        public double SummaryResult { get; set; }
        public List<FixReport> Fixs { get; set; }
    }

    public class FixReport
    {
        public FixReport(int number)
        {
            Number = number;
            Elements = new List<ElementReport>();
        }

        public int Number { get; set; }
        public List<ElementReport> Elements { get; set; }
    }

    public class ElementReport
    {
        public string Code { get; set; }
        public double Weight { get; set; }
        public double Price { get; set; }
        public double Result { get; set; }
    }
}