using Application.Dto;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FixController : ControllerBase
    {
        private readonly IFixService _fixService;

        public FixController(IFixService fixService)
        {
            _fixService = fixService;
        }

        [HttpGet("Get")]
        public CollectionResult<FixDto> Get(int? shipmentId, int? customerId, string fixDate, bool inculdeElements, int? pageNo = 1, int? pageSize = Constants.DEFAULT_PAGE_SIZE)
        {
            var shipmentDto = _fixService.Get(shipmentId, customerId, fixDate, inculdeElements, pageNo, pageSize);

            var result = new CollectionResult<FixDto>();
            result.Items = shipmentDto;
            if (result.Items != null)
            {
                result.TotalCount = result.Items.Count();
            }

            return result;
        }

        [HttpPost("Create")]
        public IActionResult Create(FixDto fixDto)
        {
            var newFixDto = _fixService.Create(fixDto);
            return Ok(newFixDto);
        }

        [HttpPost("AddEditElements")]
        public IActionResult AddEtditElements(int? fixId, IList<ElementDto> elementsDto)
        {
            _fixService.AddEditElements(fixId, elementsDto);
            return Ok();
        }
    }
}