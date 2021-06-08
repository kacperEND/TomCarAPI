using Application.Dto;
using Application.Exceptions;
using Application.Interfaces;
using Application.Reports;
using Application.Reports.Models;
using Domain.Interfaces;
using Domain.Models;
using DotLiquid;
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
        private readonly IRepository<Location> _locationRepository;
        private readonly IRepository<Shipment> _shipmentRepository;
        private readonly IRepository<Element> _elementRepository;
        private readonly IRepository<CommonCode> _commonCodeRepository;
        private readonly ICommonCodeService _commonCodeService;

        public FixService(IRepository<Fix> fixRepository,
            IRepository<FixOrder> fixOrderRepository,
            IRepository<Customer> customerRepository,
            ICommonCodeService commonCodeService,
            IRepository<Location> locationRepository,
            IRepository<Shipment> shipmentRepository,
            IRepository<CommonCode> commonCodeRepository,
            IRepository<Element> elementRepository)
        {
            _fixRepository = fixRepository;
            _fixOrderRepository = fixOrderRepository;
            _customerRepository = customerRepository;
            _commonCodeService = commonCodeService;
            _locationRepository = locationRepository;
            _shipmentRepository = shipmentRepository;
            _elementRepository = elementRepository;
            _commonCodeRepository = commonCodeRepository;
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

            var paged = query.OrderBy(item => item.OrderDate).Skip(itemsToSkip.Value).Take(pageSize.Value);

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

            _fixOrderRepository.Create(newFixOrder);
            _fixOrderRepository.Flush();

            return newFixOrder.ConvertToDto();
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
            var lastFixOrderID = _fixOrderRepository.Table.OrderByDescending(u => u.Id).FirstOrDefault().Id;
            lastFixOrderID += 1;
            return Constants.Fix + "00" + lastFixOrderID.ToString();
        }

        private void ValidateFixOrder(FixOrder newFixOrder, FixOrderDto fixOrderDto)
        {
            if (
                   fixOrderDto == null ||
                   !fixOrderDto.NetWeight.HasValue ||
                   !fixOrderDto.IncurredCosts.HasValue ||
                   !fixOrderDto.OrderDate.HasValue
           )
            {
                throw new ValidationException("Brak wymaganych informacji!");
            }
            newFixOrder.IncurredCosts = fixOrderDto.IncurredCosts;
            newFixOrder.NetWeight = fixOrderDto.NetWeight;
            newFixOrder.OrderDate = fixOrderDto.OrderDate;

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

            var fixReport = GenerateFixReport(fixOrder);

            var template = fixOrder.Shipment.Description;

            var label = string.Empty;
            label += TemplateEngine.Parse(template, fixReport);

            return label;
        }

        private FixOrderReport GenerateFixReport(FixOrder fixOrder)
        {
            FixOrderReport fixOrderReport = new FixOrderReport();
            fixOrderReport.ShipmentCode = fixOrder.Shipment.Code;
            fixOrderReport.OrderDate = fixOrder.OrderDate.ToString();
            fixOrderReport.WeightUom = fixOrder.CommonCodeWeightUom.Code;
            fixOrderReport.Currency = fixOrder.CommonCodeCurrency.Code;
            fixOrderReport.NetWeight = (double)fixOrder.NetWeight;
            fixOrderReport.IncurredCosts = (decimal)fixOrder.IncurredCosts;

            int index = 1;
            foreach (var fix in fixOrder.Fixs)
            {
                FixReport fixReport = new FixReport(index);

                foreach (var element in fix.Elements)
                {
                    ElementReport elementReport = new ElementReport();
                    elementReport.Code = element.CommonCodeName.Code;
                    elementReport.Weight = (double)element.NetWeight;
                    elementReport.Price = Convert.ToDouble(element.Price);

                    elementReport.Result = elementReport.Weight * elementReport.Price;

                    fixOrderReport.SumFixs += elementReport.Result;
                    fixReport.Elements.Add(elementReport);
                }

                fixOrderReport.Fixs.Add(fixReport);
                index++;
            }

            fixOrderReport.Cost = Convert.ToDouble(fixOrder.NetWeight) * Convert.ToDouble(fixOrder.IncurredCosts);
            fixOrderReport.SummaryResult = fixOrderReport.SumFixs - fixOrderReport.Cost;

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
            }
            _fixRepository.Flush();
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
    }
}