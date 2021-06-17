using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto
{
    public class CalculationDto
    {
        public int Id { get; set; }
        public DateTime? CalculationDate { get; set; }
        public ElementDto Pt { get; set; }
        public ElementDto Pd { get; set; }
        public ElementDto Rh { get; set; }
        public ElementDto MainFirstElement { get; set; }
        public ElementDto MainSecondElement { get; set; }
        public ElementDto MainThirdElement { get; set; }
        public ElementDto SecondaryFirstElement { get; set; }
        public ElementDto SecondarySecondElement { get; set; }
        public ElementDto SecondaryThirdElement { get; set; }
        public ElementDto BonusFirstElement { get; set; }
        public ElementDto BonusSecondElement { get; set; }
        public ElementDto BonusThirdElement { get; set; }
        public double? Weight { get; set; }
        public double? Price { get; set; }
        public double? ResultPercent { get; set; }
        public double? InvoiceSum { get; set; }
        public double? InvoicePrice { get; set; }
    }

    public static class CalculationDtoExtension
    {
        public static void CopyFromDto(this Calculation calculation, CalculationDto calculationDto)
        {
            calculation.Weight = calculationDto.Weight;
            calculation.Price = calculationDto.Price;
            calculation.CalculationDate = calculationDto.CalculationDate;
            calculation.ResultPercent = calculationDto.ResultPercent;
            calculation.InvoiceSum = calculationDto.InvoiceSum;
            calculation.InvoicePrice = calculationDto.InvoicePrice;
        }

        public static CalculationDto ConvertToDto(this Calculation calculation)
        {
            var calculationDto = new CalculationDto();
            calculationDto.Id = calculation.Id;
            calculationDto.Weight = calculation.Weight;
            calculationDto.CalculationDate = calculation.CalculationDate;
            calculationDto.Price = calculation.Price;
            calculationDto.ResultPercent = calculation.ResultPercent;
            calculationDto.InvoiceSum = calculation.InvoiceSum;
            calculationDto.InvoicePrice = calculation.InvoicePrice;

            calculationDto.Pt = ElementDtoExtension.ConvertToDto(calculation.Pt);
            calculationDto.Pd = ElementDtoExtension.ConvertToDto(calculation.Pd);
            calculationDto.Rh = ElementDtoExtension.ConvertToDto(calculation.Rh);

            calculationDto.MainFirstElement = ElementDtoExtension.ConvertToDto(calculation.MainFirstElement);
            calculationDto.MainSecondElement = ElementDtoExtension.ConvertToDto(calculation.MainSecondElement);
            calculationDto.MainThirdElement = ElementDtoExtension.ConvertToDto(calculation.MainThirdElement);

            calculationDto.SecondaryFirstElement = ElementDtoExtension.ConvertToDto(calculation.SecondaryFirstElement);
            calculationDto.SecondarySecondElement = ElementDtoExtension.ConvertToDto(calculation.SecondarySecondElement);
            calculationDto.SecondaryThirdElement = ElementDtoExtension.ConvertToDto(calculation.SecondaryThirdElement);

            calculationDto.BonusFirstElement = ElementDtoExtension.ConvertToDto(calculation.BonusFirstElement);
            calculationDto.BonusSecondElement = ElementDtoExtension.ConvertToDto(calculation.BonusSecondElement);
            calculationDto.BonusThirdElement = ElementDtoExtension.ConvertToDto(calculation.BonusThirdElement);
            return calculationDto;
        }
    }
}