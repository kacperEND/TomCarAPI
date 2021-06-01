using Application.Dto;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Interfaces;
using Domain.Models;
using System.Collections.Generic;
using System.Linq;
using ValidationException = Application.Exceptions.ValidationException;

namespace Application.Services
{
    public class FixService : IFixService
    {
        private readonly IRepository<Fix> _fixRepository;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<Location> _locationRepository;
        private readonly IRepository<Shipment> _shipmentRepository;
        private readonly IRepository<Element> _elementRepository;
        private readonly IRepository<CommonCode> _commonCodeRepository;
        private readonly ICommonCodeService _commonCodeService;

        public FixService(IRepository<Fix> fixRepository,
            IRepository<Customer> customerRepository,
            ICommonCodeService commonCodeService,
            IRepository<Location> locationRepository,
            IRepository<Shipment> shipmentRepository,
            IRepository<CommonCode> commonCodeRepository,
            IRepository<Element> elementRepository)
        {
            _fixRepository = fixRepository;
            _customerRepository = customerRepository;
            _commonCodeService = commonCodeService;
            _locationRepository = locationRepository;
            _shipmentRepository = shipmentRepository;
            _elementRepository = elementRepository;
            _commonCodeRepository = commonCodeRepository;
        }

        public IEnumerable<FixDto> Get(int? shipmentId, int? customerId, string fixDate, bool inculdeElements, int? pageNo, int? pageSize)
        {
            int? itemsToSkip = (pageNo - 1) * pageSize;

            var query = this._fixRepository.Table.Where(item => !item.IsDeleted);

            if (shipmentId.HasValue)
            {
                query = query.Where(item => item.ShipmentId == shipmentId);
            }

            if (customerId.HasValue)
            {
                query = query.Where(item => item.CustomerId == customerId);
            }

            var paged = query.OrderBy(item => item.FixDate).Skip(itemsToSkip.Value).Take(pageSize.Value);

            var listOfFixes = paged.ToList().Select(item => item.ConvertToDto(inculdeElements));
            return listOfFixes;
        }

        public FixDto Create(FixDto fixDto)
        {
            Fix newFix = new Fix();
            ValidateFix(newFix, fixDto);

            _fixRepository.Create(newFix);
            _fixRepository.Flush();
            return newFix.ConvertToDto();
        }

        private void ValidateFix(Fix newFix, FixDto fixDto)
        {
            if (
                   fixDto == null ||
                   !fixDto.NetWeight.HasValue ||
                   !fixDto.IncurredCosts.HasValue ||
                   !fixDto.CurrencyRates.HasValue ||
                   !fixDto.FixDate.HasValue
           )
            {
                throw new ValidationException("Brak wymaganych informacji!");
            }
            newFix.IncurredCosts = fixDto.IncurredCosts;
            newFix.NetWeight = fixDto.NetWeight;
            newFix.FixDate = fixDto.FixDate;

            var shipment = _shipmentRepository.Get(fixDto.ShipmentId);
            if (shipment == null) throw new RecordNotFoundException("Nie znaleziono Shipmentu!");
            newFix.ShipmentId = fixDto.ShipmentId;

            var customer = _customerRepository.Get(fixDto.CustomerId);
            if (customer == null) throw new RecordNotFoundException("Nie znaleziono klienta!");
            newFix.CustomerId = fixDto.CustomerId;

            if (!string.IsNullOrEmpty(fixDto.WeightUomCode))
            {
                var commonCodeWeightUom = this._commonCodeService.Query(typeof(Constants.CommonCode.WeightUom).Name, fixDto.WeightUomCode).FirstOrDefault();
                if (commonCodeWeightUom == null)
                    throw new RecordNotFoundException("Nie znaleziono typu wagi!");

                newFix.CommonCodeCurrencyId = commonCodeWeightUom.Id;
            }

            if (!string.IsNullOrEmpty(fixDto.CurrencyCode))
            {
                var commonCodeCurrency = this._commonCodeService.Query(typeof(Constants.CommonCode.Currencies).Name, fixDto.CurrencyCode).FirstOrDefault();
                if (commonCodeCurrency == null)
                    throw new RecordNotFoundException("Nie znaleziono waluty!");

                newFix.CommonCodeCurrencyId = commonCodeCurrency.Id;
            }
        }

        public void AddEditElements(int? fixId, IList<ElementDto> elementsDto)
        {
            if (elementsDto == null) return;

            var fix = _fixRepository.Get(fixId);
            if (fix == null) throw new RecordNotFoundException("Nie znaleziono Fixu!");

            var isElementValid = elementsDto.Any(item => !item.Price.HasValue || !item.NameCodeId.HasValue);
            if (isElementValid) throw new ValidationException("Brak wymaganych informacji!");

            foreach (ElementDto elementDto in elementsDto)
            {
                var commonCodeName = _commonCodeRepository.Get(elementDto.NameCodeId);
                if (commonCodeName == null)
                    throw new ValidationException($"Metal <strong>{elementDto.NameCodeId}</strong> nie istnieje");

                if (elementDto.Id == 0)
                {
                    var newElement = new Element();
                    newElement.CommonCodeNameId = commonCodeName.Id;
                    newElement.Price = elementDto.Price;
                    newElement.FixId = fix.Id;
                    _elementRepository.Create(newElement);
                }
                else
                {
                    var existingElement = _elementRepository.Get(elementDto.Id);
                    if (existingElement.FixId != fix.Id)
                        throw new ValidationException("Brak zgodności danych!");

                    existingElement.Price = elementDto.Price;
                    existingElement.CommonCodeNameId = elementDto.NameCodeId;
                    _elementRepository.Update(existingElement);
                }
            }

            _elementRepository.Flush();
        }

        public string GenerateFixReport(int? fixId)
        {
            var fix = this._fixRepository.Get(fixId);
            if (fix == null)
                throw new RecordNotFoundException("Nie znaleziono Fixu!");

            var fixReport = GenerateFixReport(fix);

            var template = "";

            var label = string.Empty;
            //label += this._templateEngine.Parse(template, fixReport);

            return label;
        }

        private object GenerateFixReport(Fix fix)
        {
            throw new System.NotImplementedException();
        }
    }
}