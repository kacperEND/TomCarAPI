using Application.Dto;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Interfaces;
using Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IRepository<Customer> _customerRepository;

        public CustomerService(IRepository<Customer> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public CustomerDto Get(int id)
        {
            var customer = _customerRepository.Get(id);
            if (customer == null)
                throw new RecordNotFoundException("Brak klienta w bazie!");

            return customer.ConvertToDto();
        }

        public IEnumerable<CustomerDto> SearchCustomers(string searchTerm, int? pageNo, int? pageSize)
        {
            int? itemsToSkip = (pageNo - 1) * pageSize;

            var query = this._customerRepository.Table.Where(item => !item.IsDeleted);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                string sanatizedSearchTerm = searchTerm.Trim().ToUpper();
                query = query.Where(item => item.CompanyName.ToUpper().Contains(sanatizedSearchTerm) || item.Name.ToUpper().Contains(sanatizedSearchTerm) || item.Email.ToUpper().Contains(sanatizedSearchTerm));
            }
            var pagedCustomers = query.OrderBy(item => item.Name).Skip(itemsToSkip.Value).Take(pageSize.Value);

            var listOfCustomers = pagedCustomers.ToList().Select(item => item.ConvertToDto());
            return listOfCustomers;
        }

        public CustomerDto CreateNewCustomer(CustomerDto customerDto)
        {
            var newCustomer = new Customer();
            newCustomer.CopyFromDto(customerDto);

            var isCustomerExists = _customerRepository.Table.Any(item => item.Name == newCustomer.Name
            //&& item.SecondName == newCustomer.SecondName
            && item.CompanyName == newCustomer.CompanyName);
            if (isCustomerExists)
                throw new ValidationException("Klient już istnieje!");

            _customerRepository.Create(newCustomer);
            _customerRepository.Flush();

            return newCustomer.ConvertToDto();
        }

        public void SoftDeleteCustomer(int id)
        {
            var customer = _customerRepository.Get(id);
            if (customer == null)
                throw new ValidationException("Brak klienta w bazie!");

            customer.IsDeleted = true;
            _customerRepository.Update(customer);
            _customerRepository.Flush();
        }

        public CustomerDto Update(CustomerDto customerDto)
        {
            if (
                   customerDto == null ||
                   customerDto.Id <= 0 ||
                   customerDto.LocationId <= 0 ||
                   string.IsNullOrWhiteSpace(customerDto.Name)
           )
            {
                throw new ValidationException("Brak wymaganych informacji!");
            }

            var existingCustomerFromDatabase = _customerRepository.Get(customerDto.Id);
            if (existingCustomerFromDatabase == null)
                throw new RecordNotFoundException("Brak klienta w bazie!");

            existingCustomerFromDatabase.Name = customerDto.Name;
            existingCustomerFromDatabase.FirstName = customerDto.FirstName;
            existingCustomerFromDatabase.CompanyName = customerDto.CompanyName;
            existingCustomerFromDatabase.Email = customerDto.Email;
            existingCustomerFromDatabase.TaxNumber = customerDto.TaxNumber;
            existingCustomerFromDatabase.LocationId = customerDto.LocationId;
            existingCustomerFromDatabase.Phone = customerDto.Phone;

            _customerRepository.Update(existingCustomerFromDatabase);
            _customerRepository.Flush();

            return existingCustomerFromDatabase.ConvertToDto();
        }
    }
}