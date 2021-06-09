using Application.Dto;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using WebAPI.DtoModels;

namespace WebAPI.Controllers
{
    [Authorize(Policy = "ApiReader")]
    [Route("api/[controller]")]
    [ApiController]
    public class FixController : ControllerBase
    {
        private readonly IFixService _fixService;

        public FixController(IFixService fixService)
        {
            _fixService = fixService;
        }

        [HttpPost("CreateFixOrder")]
        public IActionResult CreateFixOrder(FixOrderDto fixDto)
        {
            var newFixDto = _fixService.Create(fixDto);
            return Ok(newFixDto);
        }

        [HttpPut("UpdateFixOrder")]
        public IActionResult UpdateFixOrder(FixOrderDto fixDto)
        {
            var newFixDto = _fixService.UpdateFixOrder(fixDto);
            return Ok(newFixDto);
        }

        [HttpGet("GetFixOrders")]
        public CollectionResult<FixOrderDto> GetFixOrders(int? shipmentId, int? customerId, string fixDate, bool inculdeElements = false, int? pageNo = 1, int? pageSize = Constants.DEFAULT_PAGE_SIZE)
        {
            var shipmentDto = _fixService.Get(shipmentId, customerId, fixDate, inculdeElements, pageNo, pageSize);

            var result = new CollectionResult<FixOrderDto>();
            result.Items = shipmentDto;
            if (result.Items != null)
            {
                result.TotalCount = result.Items.Count();
            }

            return result;
        }

        [HttpGet("Get")]
        public CollectionResult<FixOrderDto> Get(int? shipmentId, int? customerId, string fixDate, bool inculdeElements, int? pageNo = 1, int? pageSize = Constants.DEFAULT_PAGE_SIZE)
        {
            var shipmentDto = _fixService.Get(shipmentId, customerId, fixDate, inculdeElements, pageNo, pageSize);

            var result = new CollectionResult<FixOrderDto>();
            result.Items = shipmentDto;
            if (result.Items != null)
            {
                result.TotalCount = result.Items.Count();
            }

            return result;
        }

        [HttpGet("GetFixs")]
        public CollectionResult<FixDto> GetFixs(int? fixOrderId)
        {
            var fixDtos = _fixService.GetFixs(fixOrderId);

            var result = new CollectionResult<FixDto>();
            result.Items = fixDtos;
            if (result.Items != null)
            {
                result.TotalCount = result.Items.Count();
            }

            return result;
        }

        [HttpPost("AddEditFixs/{fixOrderId}")]
        public IActionResult AddEditFixs(int? fixOrderId, IList<FixDto> fixsDto)
        {
            _fixService.AddEditFixs(fixOrderId, fixsDto);
            return Ok();
        }

        [HttpPost("AddEditElements")]
        public IActionResult AddEditElements(int? fixOrderId, int? fixId, IList<ElementDto> elementsDto)
        {
            _fixService.AddEditFixsElements(fixId, elementsDto);
            return Ok();
        }

        [HttpGet("GenerateFixOrderReport/{fixOrderId}")]
        public IActionResult GenerateFixOrderReport(int? fixOrderId)
        {
            var fixOrderReportLabel = _fixService.GenerateFixOrderReport(fixOrderId);
            var reportDto = new ReportDto
            {
                Name = "FixOrderDto",
                Body = fixOrderReportLabel
            };

            return Ok(reportDto); ;
        }
    }
}