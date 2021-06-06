using Application.Dto;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShipmentsController : ControllerBase
    {
        private readonly IShipmentService _shipmentService;

        public ShipmentsController(IShipmentService shipmentService)
        {
            _shipmentService = shipmentService;
        }

        [HttpGet("Get")]
        public CollectionResult<ShipmentDto> Get(string shipmentStatus = null, string searchTerm = null, string startDate = null, string endDate = null, int? pageNo = 1, int? pageSize = Constants.DEFAULT_PAGE_SIZE)
        {
            var shipmentDto = _shipmentService.Get(shipmentStatus, searchTerm, startDate, endDate, pageNo, pageSize);

            var result = new CollectionResult<ShipmentDto>();
            result.Items = shipmentDto;
            if (result.Items != null)
            {
                result.TotalCount = result.Items.Count();
            }

            return result;
        }

        [HttpGet("Get/{id}")]
        public IActionResult Get(int id)
        {
            var shipmentDto = _shipmentService.Get(id);
            return Ok(shipmentDto);
        }

        [HttpPost("Create")]
        public IActionResult Create(ShipmentDto shipmentDto)
        {
            var newShipmentDto = _shipmentService.Create(shipmentDto);
            return Ok(newShipmentDto);
        }
    }
}