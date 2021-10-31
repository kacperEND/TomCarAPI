namespace Application
{
    public static class Constants
    {
        public const string Fix = "FIX";
        public const int DEFAULT_PAGE_SIZE = 10;

        public static class LabelTemplates
        {
            public const string FixOrderReportTemplate = "FixOrderReportTemplate";
            public const string CalculationReportTemplate = "CalculationReportTemplate";
        }

        public static class CommonCode
        {
            public static class Currencies
            {
                public const string USDollars = "USD";
                public const string Euro = "Euro";
                public const string PLN = "PLN";
            }

            public static class ChemicalElementName
            {
                public const string Platinium = "PT";
                public const string Palladium = "PD";
                public const string Rhodium = "RH";
            }

            public static class WeightUom
            {
                public const string Gram = "g";
                public const string Kilogram = "kg";
            }
        }
    }
}