using Application.Dto;
using Application.Interfaces;
using Domain.Interfaces;
using Domain.Models;
using System.Linq;

namespace Application.Services
{
    public class LocationService : ILocationService
    {
        private readonly IRepository<Location> _locationRepository;

        public LocationService(IRepository<Location> locationRepository)
        {
            _locationRepository = locationRepository;
        }

        public CollectionResult<LocationDto> SearchLocations(string searchTerm)
        {
            var query = this._locationRepository.Table.Where(item => !item.IsDeleted);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                string sanatizedSearchTerm = searchTerm.Trim().ToUpper();
                query = query.Where(item => item.Name.ToUpper().Contains(sanatizedSearchTerm));
            }

            var result = new CollectionResult<LocationDto>();
            result.Items = query.ToList().OrderBy(item => item.Name).Select(item => item.ConvertToDto());

            if (result.Items != null)
            {
                result.TotalCount = result.Items.Count();
            }

            return result;
        }
    }
}