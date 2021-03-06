using Application.Exceptions;
using Application.Interfaces;
using Domain.Interfaces;
using Domain.Models.MongoDB;
using Domain.Models.MongoDB.Core;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using WebAPI.DtoModels;

namespace Application.Services
{
    public class ShipmentLiteService
    {
        private readonly IMongoRepository<Shipment> _shipmentLiteRepository;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ILogService _logService;

        public ShipmentLiteService(IMongoRepository<Shipment> shipmentLiteRepository, IHttpContextAccessor contextAccessor, MongoLogService logService)
        {
            _shipmentLiteRepository = shipmentLiteRepository;
            _contextAccessor = contextAccessor;
            _logService = logService;
        }

        public IEnumerable<ShipmenLiteDto> Find(string searchterm = "")
        {
            searchterm = searchterm ?? "";
            bool isShipmentNumber = int.TryParse(searchterm, out int shipmentNoTerm);

            var filter = isShipmentNumber
                ? Builders<Shipment>.Filter.Where(x => x.ShipmentNo != null && x.ShipmentNo == shipmentNoTerm)
                : Builders<Shipment>.Filter.Where(x => x.CompanyName.ToUpper().Contains(searchterm.ToUpper()));

            var fixs = _shipmentLiteRepository.Collection.Find(filter).Limit(Constants.DEFAULT_PAGE_SIZE).ToList();

            return fixs.OrderByDescending(item => DateTime.Parse(item.Date)).Select(item => item.ConvertToDto());
        }

        public Shipment Get(string shipmentId)
        {
            var shipment = _shipmentLiteRepository.Get(shipmentId);
            if (shipment is null)
            {
                throw new ValidationException("Brak dokumentu w bazie!");
            }

            return shipment;
        }

        public void Update(Shipment shipment)
        {
            var userEmail = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email).Value;

            shipment.Info = shipment.Info ?? new BasicInfo();

            shipment.Info.DateModified = DateTime.UtcNow;
            shipment.Info.ModifiedBy = userEmail;
            _shipmentLiteRepository.UpdateAsync(shipment);
        }

        public void Remove(string shipmentId) =>
             _shipmentLiteRepository.DeleteAsync(shipmentId);

        public Shipment CreateNewShipment(Shipment shipment = null)
        {
            var newShipment = shipment.ShipmentNo.HasValue ? shipment :GenerateShipmentFromTemplate();
            _shipmentLiteRepository.Create(newShipment);

            var userEmail = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email).Value;
            _logService.Info("Create ShipmentLite", userEmail, $"ShipmentNo: {newShipment.ShipmentNo}");

            return newShipment;
        }

        private Shipment GenerateShipmentFromTemplate()
        {
            var userEmail = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email).Value;

            var shipment = new Shipment
            {
                Date = DateTime.Now.ToString("yyyy'-'MM'-'dd"),
                CompanyName = "{Nazwa}",
                Revision = new Revision(),
                FixLite = new FixLite
                {
                    _PT_Percent = 98,
                    _PD_Percent = 98,
                    _RH_Percent = 88
                },
                Info = new BasicInfo
                {
                    CreatedBy = userEmail,
                    DateCreated = DateTime.UtcNow,
                    IsDeleted = false,
                }
            };

            return shipment;
        }

    }
}