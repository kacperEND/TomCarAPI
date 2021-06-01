using Application.Dto;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationsController : ControllerBase
    {
        private readonly ILocationService _locationService;

        public LocationsController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        [HttpGet("SearchLocations")]
        public CollectionResult<LocationDto> SearchCustomers(string searchTerm = null)
        {
            CollectionResult<LocationDto> customerDto = _locationService.SearchLocations(searchTerm);

            return customerDto;
        }
    }
}