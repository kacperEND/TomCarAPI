using Application.Dto;
using Domain.Models;
using System.Collections.Generic;

namespace Application.Interfaces
{
    public interface IFixService
    {
        void AddEditElements(int? fixId, IList<ElementDto> elementsDto);

        FixDto Create(FixDto fixDto);

        string GenerateFixReport(int? fixId);

        IEnumerable<FixDto> Get(int? shipmentId, int? customerId, string fixDate, bool inculdeElements, int? pageNo, int? pageSize);
    }
}