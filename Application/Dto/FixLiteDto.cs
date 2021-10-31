using Domain.Models.MongoDB;
using System;

namespace WebAPI.DtoModels
{
    public class FixLiteDto
    {
        public string Id { get; set; }
        public int? Shipment { get; set; }
        public string Date { get; set; }
        public string CompanyName { get; set; }
    }

    public static class FixLiteExtension
    {
        public static FixLiteDto ConvertToDto(this FixLite model)
        {
            return new FixLiteDto
            {
                Id = model.Id,
                Shipment = model.Shipment,
                Date = model.Date,
                CompanyName = model.CompanyName,
            };
        }
    }
}