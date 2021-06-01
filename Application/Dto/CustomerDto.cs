using Domain.Models;

namespace Application.Dto
{
    public class CustomerDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CompanyName { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int? LocationId { get; set; }
        public string LocationName { get; set; }
        public string TaxNumber { get; set; }
    }

    public static class CustomerDtoExtension
    {
        public static CustomerDto ConvertToDto(this Customer customer)
        {
            return new CustomerDto
            {
                Id = customer.Id,
                Name = customer.Name,
                CompanyName = customer.CompanyName,
                Email = customer.Email,
                Phone = customer.Phone,
                FirstName = customer.FirstName,
                SecondName = customer.SecondName,
                LocationId = customer.LocationId,
                LocationName = customer.Location?.Name,
                TaxNumber = customer.TaxNumber,
            };
        }

        public static void CopyFromDto(this Customer customerModel, CustomerDto customerDto)
        {
            customerModel.Id = customerDto.Id;
            customerModel.Name = customerDto.Name;
            customerModel.CompanyName = customerDto.CompanyName;
            customerModel.Email = customerDto.Email;
            customerModel.FirstName = customerDto.FirstName;
            customerModel.SecondName = customerDto.SecondName;
            customerModel.Phone = customerDto.Phone;
            customerModel.LocationId = customerDto.LocationId;
            customerModel.TaxNumber = customerDto.TaxNumber;
        }
    }
}