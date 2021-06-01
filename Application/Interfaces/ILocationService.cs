using Application.Dto;

namespace Application.Interfaces
{
    public interface ILocationService
    {
        CollectionResult<LocationDto> SearchLocations(string searchTerm);
    }
}