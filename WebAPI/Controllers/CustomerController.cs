using Application.Dto;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet("SearchCustomers")]
        public CollectionResult<CustomerDto> SearchCustomers(string searchTerm = null, int? pageNo = 1, int? pageSize = Constants.DEFAULT_PAGE_SIZE)
        {
            var listOfCustomersDto = _customerService.SearchCustomers(searchTerm, pageNo, pageSize);

            var result = new CollectionResult<CustomerDto>();
            result.Items = listOfCustomersDto;
            if (result.Items != null)
            {
                result.TotalCount = result.Items.Count();
            }

            return result;
        }

        [HttpGet("Get/{id}")]
        public IActionResult Get(int id)
        {
            var customerDto = _customerService.Get(id);
            return Ok(customerDto);
        }

        [HttpPost("Create")]
        public IActionResult Create(CustomerDto customerDto)
        {
            var newCustomer = _customerService.CreateNewCustomer(customerDto);
            return Ok(newCustomer);
        }

        [HttpPut("Update")]
        public IActionResult Update(CustomerDto customerDto)
        {
            var newCustomer = _customerService.Update(customerDto);
            return Ok(newCustomer);
        }

        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            _customerService.SoftDeleteCustomer(id);
            return Ok();
        }
    }
}