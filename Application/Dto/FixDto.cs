using Domain.Models;
using System;
using System.Collections.Generic;

namespace Application.Dto
{
    public class FixDto
    {
        public int Id { get; set; }
        public double? NetWeight { get; set; }
        public decimal? IncurredCosts { get; set; }
        public decimal? CurrencyRates { get; set; }
        public int? ShipmentId { get; set; }
        public int? CustomerId { get; set; }
        public string CustomerName { get; set; }
        public DateTime? FixDate { get; set; }
        public int? CurrencyId { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencyCode { get; set; }
        public int? WeightUomId { get; set; }
        public string WeightUomName { get; set; }
        public string WeightUomCode { get; set; }
        public int? LocationId { get; set; }
        public string LocationName { get; set; }

        public IList<ElementDto> Elements { get; set; }
    }

    public static class FixDtoExtension
    {
        public static void CopyFromDto(this Fix fix, FixDto fixDto)
        {
            fix.Id = fixDto.Id;
            fix.NetWeight = fixDto.NetWeight;
            fix.IncurredCosts = fixDto.IncurredCosts;
            fix.ShipmentId = fixDto.ShipmentId;
            fix.CustomerId = fixDto.CustomerId;
            fix.FixDate = fixDto.FixDate;
            fix.CommonCodeCurrencyId = fixDto.CurrencyId;
            fix.CommonCodeWeightUomId = fixDto.WeightUomId;
        }

        public static FixDto ConvertToDto(this Fix fix, bool includeDetails = false)
        {
            var elemetsDto = new List<ElementDto>();
            if (includeDetails)
            {
                foreach (var element in fix.Elements)
                {
                    var dto = element.ConvertToDto();
                    elemetsDto.Add(dto);
                }
            }

            var fixDto = new FixDto();
            fixDto.Id = fix.Id;
            fixDto.NetWeight = fix.NetWeight;
            fixDto.IncurredCosts = fix.IncurredCosts;
            fixDto.CurrencyRates = fix.CurrencyRates;
            fixDto.ShipmentId = fix.ShipmentId;
            fixDto.CustomerId = fix.CustomerId;
            fixDto.CustomerName = fix.Customer?.Name;
            fixDto.FixDate = fix.FixDate;
            fixDto.CurrencyId = fix.CommonCodeCurrencyId.HasValue ? fix.CommonCodeCurrencyId : null;
            fixDto.CurrencyName = fix.CommonCodeCurrency?.Name;
            fixDto.CurrencyCode = fix.CommonCodeCurrency?.Code;
            fixDto.WeightUomId = fix.CommonCodeWeightUomId.HasValue ? fix.CommonCodeWeightUomId : null;
            fixDto.WeightUomName = fix.CommonCodeCurrency?.Name;
            fixDto.WeightUomCode = fix.CommonCodeCurrency?.Code;
            fixDto.LocationId = fix.Customer?.Location?.Id;
            fixDto.LocationName = fix.Customer?.Location?.Name;
            fixDto.Elements = elemetsDto;

            return fixDto;
        }
    }
}