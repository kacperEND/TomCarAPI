using Application.Dto;
using System.Collections.Generic;

namespace Application.Interfaces
{
    public interface IFixService
    {
        void AddEditFixsElements(int? fixId, IList<ElementDto> elementsDto);

        FixOrderDto Create(FixOrderDto fixDto);

        string GenerateFixOrderReport(int? fixId);

        IEnumerable<FixOrderDto> Get(int? shipmentId, int? customerId, string fixDate, bool inculdeElements, int? pageNo, int? pageSize);

        void AddEditFixs(int? fixOrderId, IList<FixDto> fixsDto);

        IEnumerable<FixDto> GetFixs(int? fixOrderId);

        FixOrderDto UpdateFixOrder(FixOrderDto fixDto);
        string GenerateCalculationReport(int? calculationId);
        CalculationDto UpdateCalculation(CalculationDto calculationDto);
    }
}