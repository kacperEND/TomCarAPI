﻿using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    public class ElementController : ControllerBase
    {
        private readonly IElementService _elementService;

        public ElementController(IElementService elementService)
        {
            _elementService = elementService;
        }

        //[HttpPost("Create")]
        //public IActionResult Create(FixDto fixDto)
        //{
        //    //var newFixDto = _elementService.Create(fixDto);
        //    return Ok();
        //}
    }
}