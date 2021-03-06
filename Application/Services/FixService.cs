using Application.Dto;
using Application.Exceptions;
using Application.Interfaces;
using Application.Reports;
using Application.Reports.Models;
using Domain.Interfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using ValidationException = Application.Exceptions.ValidationException;

namespace Application.Services
{
    public class FixService : IFixService
    {
        private readonly IRepository<Fix> _fixRepository;
        private readonly IRepository<FixOrder> _fixOrderRepository;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<Shipment> _shipmentRepository;
        private readonly IRepository<Calculation> _calculationRepository;
        private readonly IRepository<Element> _elementRepository;
        private readonly IAppConfigService _appConfigService;
        private readonly ICommonCodeService _commonCodeService;

        public FixService(IRepository<Fix> fixRepository,
            IRepository<FixOrder> fixOrderRepository,
            IRepository<Customer> customerRepository,
            ICommonCodeService commonCodeService,
            IRepository<Shipment> shipmentRepository,
            IRepository<Element> elementRepository,
            IAppConfigService appConfigService,
            IRepository<Calculation> calculationRepository)
        {
            _fixRepository = fixRepository;
            _fixOrderRepository = fixOrderRepository;
            _customerRepository = customerRepository;
            _commonCodeService = commonCodeService;
            _shipmentRepository = shipmentRepository;
            _elementRepository = elementRepository;
            _appConfigService = appConfigService;
            _calculationRepository = calculationRepository;
        }

        public IEnumerable<FixOrderDto> Get(int? shipmentId, int? customerId, string fixDate, bool inculdeElements, int? pageNo, int? pageSize)
        {
            int? itemsToSkip = (pageNo - 1) * pageSize;

            var query = this._fixOrderRepository.Table.Where(item => !item.IsDeleted);

            if (shipmentId.HasValue)
            {
                query = query.Where(item => item.ShipmentId == shipmentId);
            }

            if (customerId.HasValue)
            {
                query = query.Where(item => item.CustomerId == customerId);
            }

            var paged = query.OrderByDescending(item => item.OrderDate).Skip(itemsToSkip.Value).Take(pageSize.Value);

            var listOfFixes = paged.ToList().Select(item => item.ConvertToDto(inculdeElements));
            return listOfFixes;
        }

        public IEnumerable<FixDto> GetFixs(int? fixOrderId)
        {
            var query = this._fixRepository.Table.Where(item => !item.IsDeleted && item.FixOrderId == fixOrderId).OrderBy(item => item.Id);
            var listOfFixes = query.ToList().Select(item => item.ConvertToDto(true));

            return listOfFixes;
        }

        public FixOrderDto Create(FixOrderDto fixDto)
        {
            FixOrder newFixOrder = new FixOrder();
            ValidateFixOrder(newFixOrder, fixDto);
            newFixOrder.Number = GenerateFixOrderNumber();

            var calc = GenerateNewCalculation();
            newFixOrder.CalculationId = calc.Id;

            _fixOrderRepository.Create(newFixOrder);
            _fixOrderRepository.Flush();

            return newFixOrder.ConvertToDto();
        }

        private Calculation GenerateNewCalculation()
        {
            var ptCode = _commonCodeService.Get(typeof(Constants.CommonCode.ChemicalElementName).Name, Constants.CommonCode.ChemicalElementName.Platinium);
            var pdCode = _commonCodeService.Get(typeof(Constants.CommonCode.ChemicalElementName).Name, Constants.CommonCode.ChemicalElementName.Palladium);
            var rhCode = _commonCodeService.Get(typeof(Constants.CommonCode.ChemicalElementName).Name, Constants.CommonCode.ChemicalElementName.Rhodium);

            var calculation = new Calculation();
            calculation.Pt = new Element { CommonCodeNameId = ptCode.Id, Percent = 0.98 };
            calculation.Pd = new Element { CommonCodeNameId = pdCode.Id, Percent = 0.98 };
            calculation.Rh = new Element { CommonCodeNameId = rhCode.Id, Percent = 0.88 };

            calculation.MainFirstElement = new Element { CommonCodeNameId = ptCode.Id };
            calculation.MainSecondElement = new Element { CommonCodeNameId = pdCode.Id };
            calculation.MainThirdElement = new Element { CommonCodeNameId = rhCode.Id };

            calculation.SecondaryFirstElement = new Element { CommonCodeNameId = ptCode.Id };
            calculation.SecondarySecondElement = new Element { CommonCodeNameId = pdCode.Id };
            calculation.SecondaryThirdElement = new Element { CommonCodeNameId = rhCode.Id };

            calculation.BonusFirstElement = new Element { CommonCodeNameId = ptCode.Id };
            calculation.BonusSecondElement = new Element { CommonCodeNameId = pdCode.Id };
            calculation.BonusThirdElement = new Element { CommonCodeNameId = rhCode.Id };

            calculation.ResultPercent = 0;

            _calculationRepository.Create(calculation);
            _calculationRepository.Flush();

            return calculation;
        }

        public FixOrderDto UpdateFixOrder(FixOrderDto fixDto)
        {
            var existingFixOrder = _fixOrderRepository.Get(fixDto.Id);
            if (existingFixOrder == null) throw new RecordNotFoundException("Nie znaleziono zamówienia Fixu!");

            ValidateFixOrder(existingFixOrder, fixDto);

            _fixOrderRepository.Update(existingFixOrder);
            _fixOrderRepository.Flush();

            return existingFixOrder.ConvertToDto();
        }

        private string GenerateFixOrderNumber()
        {
            var lastFixOrderID = 1;
            if (_fixOrderRepository.Table.Any(item => item.Id > 0))
            {
                lastFixOrderID = _fixOrderRepository.Table.OrderByDescending(u => u.Id).FirstOrDefault().Id;
                lastFixOrderID += 1;
            }

            return Constants.Fix + "00" + lastFixOrderID.ToString();
        }

        private void ValidateFixOrder(FixOrder newFixOrder, FixOrderDto fixOrderDto)
        {
            if (fixOrderDto == null)
            {
                throw new ValidationException("Brak wymaganych informacji!");
            }

            newFixOrder.IncurredCosts = fixOrderDto.IncurredCosts;
            newFixOrder.NetWeight = fixOrderDto.NetWeight;
            newFixOrder.OrderDate = fixOrderDto.OrderDate;
            newFixOrder.Description = fixOrderDto.Description;
            newFixOrder.AdditionalFieldName = fixOrderDto.AdditionalFieldName;
            newFixOrder.AdditionalFieldValue = fixOrderDto.AdditionalFieldValue;

            var shipment = _shipmentRepository.Get(fixOrderDto.ShipmentId);
            if (shipment == null) throw new RecordNotFoundException("Nie znaleziono Shipmentu!");
            newFixOrder.ShipmentId = fixOrderDto.ShipmentId;

            var customer = _customerRepository.Get(fixOrderDto.CustomerId);
            if (customer == null) throw new RecordNotFoundException("Nie znaleziono klienta!");
            newFixOrder.CustomerId = fixOrderDto.CustomerId;

            if (!string.IsNullOrEmpty(fixOrderDto.WeightUomCode))
            {
                var commonCodeWeightUom = this._commonCodeService.Query(typeof(Constants.CommonCode.WeightUom).Name, fixOrderDto.WeightUomCode).FirstOrDefault();
                if (commonCodeWeightUom == null)
                    throw new RecordNotFoundException("Nie znaleziono typu wagi!");

                newFixOrder.CommonCodeWeightUomId = commonCodeWeightUom.Id;
            }

            if (!string.IsNullOrEmpty(fixOrderDto.CurrencyCode))
            {
                var commonCodeCurrency = this._commonCodeService.Query(typeof(Constants.CommonCode.Currencies).Name, fixOrderDto.CurrencyCode).FirstOrDefault();
                if (commonCodeCurrency == null)
                    throw new RecordNotFoundException("Nie znaleziono waluty!");

                newFixOrder.CommonCodeCurrencyId = commonCodeCurrency.Id;
            }
        }

        public string GenerateFixOrderReport(int? fixOrderId)
        {
            var fixOrder = this._fixOrderRepository.Get(fixOrderId);
            if (fixOrder == null)
                throw new RecordNotFoundException("Nie znaleziono Fixu!");

            var fixReport = GenerateFixReportModel(fixOrder);

            var template = string.Empty;
            template = _appConfigService.Get(Constants.LabelTemplates.FixOrderReportTemplate);

            var label = string.Empty;
            label += TemplateEngine.Parse(template, fixReport);

            return label;
        }

        private FixOrderReport GenerateFixReportModel(FixOrder fixOrder)
        {
            FixOrderReport fixOrderReport = new FixOrderReport();
            fixOrderReport.ShipmentCode = fixOrder.Shipment.Code;
            fixOrderReport.CustomerName = fixOrder.Customer.CompanyName;
            fixOrderReport.OrderDate = String.Format("{0:dd/MM/yyyy}", fixOrder.OrderDate);
            fixOrderReport.WeightUom = fixOrder.CommonCodeWeightUom.Code;
            fixOrderReport.Currency = fixOrder.CommonCodeCurrency.Code;
            fixOrderReport.NetWeight = fixOrder.NetWeight;
            fixOrderReport.IncurredCosts = ToDouble(fixOrder.IncurredCosts);
            fixOrderReport.AdditionalFieldName = fixOrder.AdditionalFieldName;
            fixOrderReport.AdditionalFieldValue = ToDouble(fixOrder.AdditionalFieldValue);

            int index = 1;
            foreach (var fix in fixOrder.Fixs)
            {
                FixReport fixReport = new FixReport(index);

                foreach (var element in fix.Elements)
                {
                    ElementReport elementReport = new ElementReport();
                    elementReport.Code = element.CommonCodeName.Code;
                    elementReport.Weight = element.NetWeight.Value;
                    elementReport.Price = ToDouble(element.Price.Value);

                    elementReport.Result = ToDouble(elementReport.Weight * elementReport.Price);

                    fixOrderReport.SumFixs += elementReport.Result;
                    fixReport.Elements.Add(elementReport);
                }

                fixOrderReport.Fixs.Add(fixReport);
                index++;
            }

            fixOrderReport.Cost = ToDouble(fixOrder.NetWeight) * ToDouble(fixOrder.IncurredCosts);
            fixOrderReport.SummaryResult = ToDouble(fixOrderReport.SumFixs - fixOrderReport.Cost);
            fixOrderReport.SummaryResultMinusAdd = ToDouble(fixOrderReport.SummaryResult - fixOrderReport.AdditionalFieldValue);

            return fixOrderReport;
        }

        public void AddEditFixs(int? fixOrderId, IList<FixDto> fixsDto)
        {
            var fixOrder = _fixOrderRepository.Get(fixOrderId);
            if (fixOrder == null) throw new RecordNotFoundException("Nie znaleziono zamówienia Fixu!");

            var oldFixes = _fixRepository.Table.Where(item => item.FixOrderId == fixOrderId && !item.IsDeleted).ToList();

            foreach (var fixDto in fixsDto)
            {
                if (fixDto.Elements != null && fixDto.Elements.Count > 0)
                {
                    var newFix = new Fix
                    {
                        FixOrderId = fixOrder.Id
                    };

                    _fixRepository.Create(newFix);
                    _fixRepository.Flush();

                    AddEditFixsElements(newFix.Id, fixDto.Elements);
                }
            }

            foreach (var oldFix in oldFixes)
            {
                _fixRepository.Delete(oldFix.Id);
                _fixRepository.Flush();
            }
        }

        public void AddEditFixsElements(int? fixId, IList<ElementDto> elementsDto)
        {
            if (elementsDto == null) return;

            var isElementValid = elementsDto.Any(item => !item.Price.HasValue || string.IsNullOrEmpty(item.NameCodeCode));
            if (isElementValid) throw new ValidationException("Brak wymaganych informacji!");

            foreach (ElementDto elementDto in elementsDto)
            {
                var commonCodeName = this._commonCodeService.Query(typeof(Constants.CommonCode.ChemicalElementName).Name, elementDto.NameCodeCode).FirstOrDefault();
                if (commonCodeName == null)
                    throw new ValidationException($"Metal <strong>{elementDto.NameCodeId}</strong> nie istnieje");

                if (elementDto.Id == 0)
                {
                    var newElement = new Element();
                    newElement.CommonCodeNameId = commonCodeName.Id;
                    newElement.Price = elementDto.Price;
                    newElement.NetWeight = elementDto.NetWeight;
                    newElement.FixId = fixId;
                    _elementRepository.Create(newElement);
                }
                else
                {
                    var existingElement = _elementRepository.Get(elementDto.Id);
                    if (existingElement.FixId != fixId)
                        throw new ValidationException("Brak zgodności danych!");

                    existingElement.Price = elementDto.Price;
                    existingElement.NetWeight = elementDto.NetWeight;
                    existingElement.CommonCodeNameId = elementDto.NameCodeId;
                    _elementRepository.Update(existingElement);
                }
            }

            _elementRepository.Flush();
        }

        public CalculationDto UpdateCalculation(CalculationDto calculationDto)
        {
            var existingCalculation = _calculationRepository.Get(calculationDto.Id);
            if (existingCalculation == null) throw new RecordNotFoundException("Nie znaleziono zamówienia obiektu!");

            UpdateElement(calculationDto.Pd);
            UpdateElement(calculationDto.Rh);
            UpdateElement(calculationDto.Pt);
            UpdateElement(calculationDto.MainFirstElement);
            UpdateElement(calculationDto.MainSecondElement);
            UpdateElement(calculationDto.MainThirdElement);
            UpdateElement(calculationDto.SecondaryFirstElement);
            UpdateElement(calculationDto.SecondarySecondElement);
            UpdateElement(calculationDto.SecondaryThirdElement);
            UpdateElement(calculationDto.BonusFirstElement);
            UpdateElement(calculationDto.BonusSecondElement);
            UpdateElement(calculationDto.BonusThirdElement);

            existingCalculation.CopyFromDto(calculationDto);
            _calculationRepository.Update(existingCalculation);
            _calculationRepository.Flush();

            return existingCalculation.ConvertToDto();
        }

        private void UpdateElement(ElementDto elementDto)
        {
            var element = _elementRepository.Get(elementDto.Id);
            element.CopyFromDto(elementDto);
            _elementRepository.Update(element);
            _elementRepository.Flush();
        }

        public string GenerateCalculationReport(int? fixOrderId)
        {
            var fixOrder = this._fixOrderRepository.Get(fixOrderId);
            if (fixOrder == null)
                throw new RecordNotFoundException("Nie znaleziono Fixu!");

            var templateModel = GenerateCalculationReportModel(fixOrder);

            var template = string.Empty;
            template = _appConfigService.Get(Constants.LabelTemplates.CalculationReportTemplate);

            var label = string.Empty;
            label += TemplateEngine.Parse(template, templateModel);

            return label;
        }

        private CalculationReport GenerateCalculationReportModel(FixOrder fixOrder)
        {
            var calculation = fixOrder.Calculation;

            CalculationReport calculationReport = new CalculationReport();
            calculationReport.ShipmentCode = fixOrder.Shipment.Code;
            calculationReport.CustomerName = fixOrder.Customer.CompanyName;
            calculationReport.CalculationDate = String.Format("{0:dd/MM/yyyy}", calculation.CalculationDate);
            calculationReport.WeightUom = fixOrder.CommonCodeWeightUom.Code;
            calculationReport.Currency = fixOrder.CommonCodeCurrency.Code;

            calculationReport.MainFirstElementWeight = calculation.MainFirstElement.NetWeight;
            calculationReport.MainSecondElementWeight = calculation.MainSecondElement.NetWeight;
            calculationReport.MainThirdElementWeight = calculation.MainThirdElement.NetWeight;

            calculationReport.SecondaryFirstElementWeight = calculation.SecondaryFirstElement.NetWeight;
            calculationReport.SecondarySecondElementWeight = calculation.SecondarySecondElement.NetWeight;
            calculationReport.SecondaryThirdElementWeight = calculation.SecondaryThirdElement.NetWeight;

            calculationReport.BonusFirstElementWeight = calculation.BonusFirstElement.NetWeight;
            calculationReport.BonusSecondElementWeight = calculation.BonusSecondElement.NetWeight;
            calculationReport.BonusThirdElementWeight = calculation.BonusThirdElement.NetWeight;

            calculationReport.PtNetWeight = calculation.Pt.NetWeight;
            calculationReport.PtPrice = ToDouble(calculation.Pt.Price);
            calculationReport.PtResult = calculation.Pt.Result;
            calculationReport.PtPercent = calculation.Pt.Percent;
            var ptSummary = calculationReport.PtNetWeight * calculationReport.PtPrice * calculationReport.PtResult * calculationReport.PtPercent;
            calculationReport.PtSummary = Math.Round(ptSummary.Value, 2);

            calculationReport.PdNetWeight = calculation.Pd.NetWeight;
            calculationReport.PdPrice = ToDouble(calculation.Pd.Price);
            calculationReport.PdResult = calculation.Pd.Result;
            calculationReport.PdPercent = calculation.Pd.Percent;
            var pdSummary = calculationReport.PdNetWeight * calculationReport.PdPrice * calculationReport.PdResult * calculationReport.PdPercent;
            calculationReport.PdSummary = Math.Round(pdSummary.Value, 2);

            calculationReport.RhNetWeight = calculation.Rh.NetWeight;
            calculationReport.RhPrice = ToDouble(calculation.Rh.Price);
            calculationReport.RhResult = calculation.Rh.Result;
            calculationReport.RhPercent = calculation.Rh.Percent;
            var rhSummary = calculationReport.RhNetWeight * calculationReport.RhPrice * calculationReport.RhResult * calculationReport.RhPercent;
            calculationReport.RhSummary = Math.Round(rhSummary.Value, 2);

            var calculationModelElementsSum = calculationReport.PtSummary + calculationReport.PdSummary + calculationReport.RhSummary;
            calculationReport.CalculationModelElementsSum = Math.Round(calculationModelElementsSum.Value, 2);

            calculationReport.Weight = calculation.Weight;
            calculationReport.Price = calculation.Price;
            var calculationModelCostsSum = calculationReport.Weight * calculationReport.Price;
            calculationReport.CalculationModelCostsSum = Math.Round(calculationModelCostsSum.Value, 2);

            calculationReport.ResultPercent = calculation.ResultPercent;
            var calculationModelResultSummary = (calculationReport.CalculationModelElementsSum - calculationReport.CalculationModelCostsSum) * (1 - calculationReport.ResultPercent / 100);
            calculationReport.CalculationModelResultSummary = Math.Round(calculationModelResultSummary.Value, 2);

            var calculationModelSummaryPrice = calculationReport.CalculationModelResultSummary / calculationReport.Weight;
            calculationReport.CalculationModelSummaryPrice = Math.Round(calculationModelSummaryPrice.Value, 2);

            calculationReport.InvoiceSum = calculation.InvoiceSum;
            calculationReport.InvoicePrice = calculation.InvoicePrice;

            var calculationModelForInvoiceResult = calculationReport.InvoiceSum * calculationReport.InvoicePrice;
            calculationReport.CalculationModelForInvoiceResult = Math.Round(calculationModelForInvoiceResult.Value, 2);

            return calculationReport;
        }

        public double ToDouble(object value)
        {
            return value == null ? 0 : ((IConvertible)value).ToDouble(null);
        }
    }
}