using Application.Dto;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Interfaces;
using Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace Application.Services
{
    public class ElementService : IElementService
    {
        private readonly IRepository<Element> _elementRepository;
        private readonly IRepository<Fix> _fixRepository;
        private readonly ICommonCodeService _commonCodeService;

        public ElementService(IRepository<Element> elementRepository, IRepository<Fix> fixRepository, ICommonCodeService commonCodeService)
        {
            _elementRepository = elementRepository;
            _fixRepository = fixRepository;
            _commonCodeService = commonCodeService;
        }

        public ElementDto Create(ElementDto elementDto)
        {
            Element newElement = new Element();

            var fix = _fixRepository.Get(elementDto.FixId);
            if (fix == null)
                throw new RecordNotFoundException("Nie znaleziono Fixu!");
            newElement.FixId = elementDto.FixId;

            var commoncodeElementName = this._commonCodeService.Query(typeof(Constants.CommonCode.WeightUom).Name, elementDto.NameCodeCode).FirstOrDefault();
            newElement.CommonCodeNameId = commoncodeElementName.Id;

            if (elementDto.Price.HasValue)
            {
                newElement.Price = elementDto.Price;
            }

            return newElement.ConvertToDto();
        }
    }
}