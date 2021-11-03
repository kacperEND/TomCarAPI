﻿using Application.Dto;
using Application.Interfaces;
using Application.Services;
using Domain.Models.MongoDB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using WebAPI.DtoModels;
using ShipmenLiteDto = WebAPI.DtoModels.ShipmenLiteDto;

namespace WebAPI.Controllers
{
    [Authorize(Policy = "ApiReader")]
    [Route("api/[controller]")]
    [ApiController]
    public class ShipmentLiteController : ControllerBase
    {
        private readonly ShipmentLiteService _shipmentLiteService;

        public ShipmentLiteController(ShipmentLiteService shipmentLiteService)
        {
            _shipmentLiteService = shipmentLiteService;
        }

        [HttpPost("Create")]
        public IActionResult Create(Shipment ship)
        {
            var result = _shipmentLiteService.Create(ship);

            return Ok(result);
        }

        [HttpGet("Get/{id}")]
        public IActionResult Get(string id)
        {
            var shipmentLite = _shipmentLiteService.Get(id);

            return Ok(shipmentLite);
        }

        [HttpGet("Find")]
        public CollectionResult<ShipmenLiteDto> Find(string searchterm = "")
        {
            var shipmentDtos = _shipmentLiteService.Find(searchterm);

            var result = new CollectionResult<ShipmenLiteDto>();
            result.Items = shipmentDtos;

            if (result.Items != null)
            {
                result.TotalCount = result.Items.Count();
            }

            return result;
        }

        [HttpPut("Update")]
        public IActionResult Update(Shipment shipment)
        {
            _shipmentLiteService.Update(shipment);

            return Ok();
        }
    }
}