using Domain.Models;
using System;
using System.Collections.Generic;

namespace Application.Dto
{
    public class FixOrderDto
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public double? NetWeight { get; set; }
        public decimal? IncurredCosts { get; set; }
        public decimal? CurrencyRates { get; set; }
        public int? ShipmentId { get; set; }
        public string ShipmentCode { get; set; }
        public int? CustomerId { get; set; }
        public string CustomerName { get; set; }
        public DateTime? OrderDate { get; set; }
        public int? CurrencyId { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencyCode { get; set; }
        public int? WeightUomId { get; set; }
        public string WeightUomName { get; set; }
        public string WeightUomCode { get; set; }
        public int? LocationId { get; set; }
        public string LocationName { get; set; }
        public string Description { get; set; }
        public string AdditionalFieldName { get; set; }
        public decimal? AdditionalFieldValue { get; set; }
        public IList<FixDto> Fixs { get; set; }
        public CalculationDto Calculation { get; set; }
    }

    public static class FixOrderDtoExtension
    {
        public static void CopyFromDto(this FixOrder fixOrder, FixOrderDto fixOrderDto)
        {
            fixOrder.Id = fixOrderDto.Id;
            fixOrder.NetWeight = fixOrderDto.NetWeight;
            fixOrder.IncurredCosts = fixOrderDto.IncurredCosts;
            fixOrder.ShipmentId = fixOrderDto.ShipmentId;
            fixOrder.CustomerId = fixOrderDto.CustomerId;
            fixOrder.OrderDate = fixOrderDto.OrderDate;
            fixOrder.CommonCodeCurrencyId = fixOrderDto.CurrencyId;
            fixOrder.CommonCodeWeightUomId = fixOrderDto.WeightUomId;
            fixOrder.Description = fixOrderDto.Description;
            fixOrder.AdditionalFieldValue = fixOrderDto.AdditionalFieldValue;
            fixOrder.AdditionalFieldName = fixOrderDto.AdditionalFieldName;
        }

        public static FixOrderDto ConvertToDto(this FixOrder fixOrder, bool includeDetails = false)
        {
            var fixsDto = new List<FixDto>();
            if (includeDetails)
            {
                foreach (var fix in fixOrder.Fixs)
                {
                    var dto = fix.ConvertToDto();
                    fixsDto.Add(dto);
                }
            }

            var fixOrderDto = new FixOrderDto();
            fixOrderDto.Id = fixOrder.Id;
            fixOrderDto.Number = fixOrder.Number;
            fixOrderDto.Description = fixOrder.Description;
            fixOrderDto.NetWeight = fixOrder.NetWeight;
            fixOrderDto.IncurredCosts = fixOrder.IncurredCosts;
            fixOrderDto.CurrencyRates = fixOrder.CurrencyRates;
            fixOrderDto.ShipmentId = fixOrder.ShipmentId;
            fixOrderDto.ShipmentCode = fixOrder.Shipment.Code;
            fixOrderDto.CustomerId = fixOrder.CustomerId;
            fixOrderDto.CustomerName = fixOrder.Customer?.CompanyName;
            fixOrderDto.OrderDate = fixOrder.OrderDate;
            fixOrderDto.AdditionalFieldValue = fixOrder.AdditionalFieldValue;
            fixOrderDto.AdditionalFieldName = fixOrder.AdditionalFieldName;
            fixOrderDto.CurrencyId = fixOrder.CommonCodeCurrencyId.HasValue ? fixOrder.CommonCodeCurrencyId : null;
            fixOrderDto.CurrencyName = fixOrder.CommonCodeCurrency?.Name;
            fixOrderDto.CurrencyCode = fixOrder.CommonCodeCurrency?.Code;
            fixOrderDto.WeightUomId = fixOrder.CommonCodeWeightUomId.HasValue ? fixOrder.CommonCodeWeightUomId : null;
            fixOrderDto.WeightUomName = fixOrder.CommonCodeWeightUom?.Name;
            fixOrderDto.WeightUomCode = fixOrder.CommonCodeWeightUom?.Code;
            fixOrderDto.LocationId = fixOrder.Customer?.Location?.Id;
            fixOrderDto.LocationName = fixOrder.Customer?.Location?.Name;
            fixOrderDto.Fixs = fixsDto;
            fixOrderDto.Calculation = CalculationDtoExtension.ConvertToDto(fixOrder.Calculation);

            return fixOrderDto;
        }
    }
}