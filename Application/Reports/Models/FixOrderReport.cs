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
        public string CustomerName { get; set; }
        public string OrderDate { get; set; }
        public string WeightUom { get; set; }
        public string Currency { get; set; }
        public double SumFixs { get; set; }
        public double NetWeight { get; set; }
        public double IncurredCosts { get; set; }
        public double Cost { get; set; }
        public double SummaryResult { get; set; }
        public string AdditionalFieldName { get; set; }
        public double AdditionalFieldValue { get; set; }
        public double SummaryResultMinusAdd { get; set; }
        public List<FixReport> Fixs { get; set; }
    }

    public class FixReport
    {
        public FixReport(int number)
        {
            Number = "FIX<br>" + number.ToString();
            Elements = new List<ElementReport>();
        }

        public string Number { get; set; }
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