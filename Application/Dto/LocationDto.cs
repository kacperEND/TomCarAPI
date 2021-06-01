using Domain.Models;

namespace Application.Dto
{
    public class LocationDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Addr1 { get; set; }
        public string Addr2 { get; set; }
        public string Addr3 { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public bool isDeleted { get; set; }
        public string IsoCountryCode { get; set; }
    }

    public static class LocationDtoExtension
    {
        public static LocationDto ConvertToDto(this Location location)
        {
            return new LocationDto
            {
                Id = location.Id,
                Name = location.Name,
                Addr1 = location.Addr1,
                Addr2 = location.Addr2,
                Addr3 = location.Addr3,
                City = location.City,
                Country = location.Country,
                isDeleted = location.IsDeleted,
                IsoCountryCode = location.IsoCountryCode,
            };
        }

        public static void CopyFromDto(this Location locationModel, LocationDto locationDto)
        {
            locationModel.Id = locationDto.Id;
            locationModel.Name = locationDto.Name;
            locationModel.Addr1 = locationDto.Addr1;
            locationModel.Addr2 = locationDto.Addr2;
            locationModel.Addr3 = locationDto.Addr3;
            locationModel.City = locationDto.City;
            locationModel.Country = locationDto.Country;
            locationModel.IsDeleted = locationDto.isDeleted;
            locationModel.IsoCountryCode = locationDto.IsoCountryCode;
        }
    }
}