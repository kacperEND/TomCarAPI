using Application.Dto;
using System.Collections.Generic;

namespace Application.Interfaces
{
    public interface IShipmentService
    {
        ShipmentDto Create(ShipmentDto shipmentDto);

        ShipmentDto Update(ShipmentDto shipmentDto);

        IEnumerable<ShipmentDto> Get(string shipmentStatus, string searchTerm, string startDate, string endDate, int? pageNo, int? pageSize);

        ShipmentDto Get(int shipmentId);
    }
}