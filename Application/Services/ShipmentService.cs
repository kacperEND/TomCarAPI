using Application.Dto;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Interfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Application.Services
{
    public class ShipmentService : IShipmentService
    {
        private readonly IRepository<Shipment> _shipmentRepository;

        public ShipmentService(IRepository<Shipment> shipmentRepository)
        {
            _shipmentRepository = shipmentRepository;
        }

        public ShipmentDto Create(ShipmentDto shipmentDto)
        {
            var isExists = _shipmentRepository.Table.Any(item => item.Code.ToUpper() == shipmentDto.Code.ToUpper() && !item.IsDeleted);
            if (isExists)
                throw new ValidationException("Już istnieje Shipment o tym numerze!");

            var newShipment = new Shipment();
            newShipment.CopyFromDto(shipmentDto);
            _shipmentRepository.Create(newShipment);
            _shipmentRepository.Flush();

            return newShipment.ConvertToDto();
        }

        public IEnumerable<ShipmentDto> Get(string shipmentStatus, string searchTerm, string startDate, string endDate, int? pageNo, int? pageSize)
        {
            int? itemsToSkip = (pageNo - 1) * pageSize;

            var query = this._shipmentRepository.Table.Where(item => !item.IsDeleted);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                string sanatizedSearchTerm = searchTerm.Trim().ToUpper();
                query = query.Where(item => item.Code.ToUpper().Contains(sanatizedSearchTerm.ToUpper()) || item.Name.ToUpper().Contains(sanatizedSearchTerm));
            }

            DateTime fromDateUtc = new DateTime();
            bool isGoodFromDate = false;
            if (!string.IsNullOrWhiteSpace(startDate))
            {
                isGoodFromDate = DateTime.TryParse(startDate, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out fromDateUtc);
                if (!isGoodFromDate)
                {
                    throw new ValidationException("Nieprawidłowy format początkowej daty");
                }
                query = query.Where(item => item.DateStart >= fromDateUtc);
            }

            DateTime endDateUtc = new DateTime();
            bool isGoodEndDate = false;
            if (!string.IsNullOrWhiteSpace(endDate))
            {
                isGoodEndDate = DateTime.TryParse(endDate, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out endDateUtc);
                if (!isGoodEndDate)
                {
                    throw new ValidationException("Nieprawidłwy format daty");
                }
                query = query.Where(item => item.DateEnd >= endDateUtc);
            }

            var pagedShipment = query.OrderBy(item => item.DateEnd).Skip(itemsToSkip.Value).Take(pageSize.Value);

            var listOfCustomers = pagedShipment.ToList().Select(item => item.ConvertToDto());
            return listOfCustomers;
        }

        public ShipmentDto Get(int shipmentId)
        {
            var customer = _shipmentRepository.Get(shipmentId);
            if (customer == null)
                throw new RecordNotFoundException("Nie znaleziono!");

            return customer.ConvertToDto();
        }

        public ShipmentDto Update(ShipmentDto shipmentDto)
        {
            throw new NotImplementedException();
        }
    }
}