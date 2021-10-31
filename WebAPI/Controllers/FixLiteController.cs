using Application.Dto;
using Application.Interfaces;
using Application.Services;
using Domain.Models.MongoDB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using WebAPI.DtoModels;

namespace WebAPI.Controllers
{
    //[Authorize(Policy = "ApiReader")]
    [Route("api/[controller]")]
    [ApiController]
    public class FixLiteController : ControllerBase
    {
        private readonly FixLiteService _fixLiteService;

        public FixLiteController(FixLiteService fixLiteService)
        {
            _fixLiteService = fixLiteService;
        }

        [HttpPost("Create")]
        public IActionResult Create(FixLite fix)
        {
            var result = _fixLiteService.Create();

            return Ok(result);
        }

        [HttpGet("Get/{id}")]
        public IActionResult Get(string id)
        {
            var fixLite = _fixLiteService.Get(id);

            return Ok(fixLite);
        }

        [HttpGet("Find")]
        public CollectionResult<FixLiteDto> Find(string searchterm = "")
        {
            var fixDtos = _fixLiteService.Find(searchterm);

            var result = new CollectionResult<FixLiteDto>();
            result.Items = fixDtos;

            if (result.Items != null)
            {
                result.TotalCount = result.Items.Count();
            }

            return result;
        }

        [HttpPut("Update")]
        public IActionResult Update(FixLite fix)
        {
            _fixLiteService.Update(fix.Id, fix);

            return Ok();
        }
    }
}