using Domain.Models.MongoDB;

namespace WebAPI.DtoModels
{
    public class ShipmenLiteDto
    {
        public string Id { get; set; }
        public int? ShipmentNo { get; set; }
        public string Date { get; set; }
        public string CompanyName { get; set; }
    }

    public static class FixLiteExtension
    {
        public static ShipmenLiteDto ConvertToDto(this Shipment model)
        {
            return new ShipmenLiteDto
            {
                Id = model.Id,
                ShipmentNo = model.ShipmentNo,
                Date = model.Date,
                CompanyName = model.CompanyName,
            };
        }
    }
}