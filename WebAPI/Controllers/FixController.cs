using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
    }
}