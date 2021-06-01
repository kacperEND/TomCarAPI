using Domain.Models;
using System;

namespace Application.Dto
{
    public class ShipmentDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
    }

    public static class ShipmentDtoExtension
    {
        public static void CopyFromDto(this Shipment shipment, ShipmentDto shipmentDto)
        {
            shipment.Name = shipmentDto.Name;
            shipment.Code = shipmentDto.Code;
            shipment.Description = shipmentDto.Description;
            shipment.DateStart = shipmentDto.DateStart.HasValue ? shipmentDto.DateStart : null;
            shipment.DateEnd = shipmentDto.DateEnd.HasValue ? shipmentDto.DateEnd : null;
        }

        public static ShipmentDto ConvertToDto(this Shipment shiment)
        {
            var dto = new ShipmentDto();
            dto.Id = shiment.Id;
            dto.Name = shiment.Name;
            dto.Code = shiment.Code;
            dto.DateStart = shiment.DateStart;
            dto.Description = shiment.Description;
            dto.DateEnd = shiment.DateEnd;
            dto.Description = shiment.Description;

            return dto;
        }
    }
}