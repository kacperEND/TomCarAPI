using Application.Dto;
using System.Collections.Generic;

namespace Application.Interfaces
{
    public interface ICustomerService
    {
        CustomerDto Get(int id);

        CustomerDto CreateNewCustomer(CustomerDto customerDto);

        CustomerDto Update(CustomerDto customerDto);

        void SoftDeleteCustomer(int id);

        IEnumerable<CustomerDto> SearchCustomers(string searchTerm, int? pageNo, int? pageSize);
    }
}