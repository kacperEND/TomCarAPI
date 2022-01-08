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

        public ShipmentLiteService(IMongoRepository<Shipment> shipmentLiteRepository, IHttpContextAccessor contextAccessor)
        {
            _shipmentLiteRepository = shipmentLiteRepository;
            _contextAccessor = contextAccessor;
        }

        public IEnumerable<ShipmenLiteDto> Find(string searchterm = "")
        {
            searchterm = searchterm ?? "";
            var isShipmentNumber = int.TryParse(searchterm, out int shipmentNoTerm);

            FilterDefinition<Shipment> filter;
            if (isShipmentNumber)
            {
                filter = Builders<Shipment>.Filter.Where(x => x.ShipmentNo != null && x.ShipmentNo == shipmentNoTerm);
            }
            else
            {
                filter = Builders<Shipment>.Filter.Where(x => x.CompanyName.ToUpper().Contains(searchterm.ToUpper()));
            }

            var fixs = _shipmentLiteRepository.Collection.Find(filter).Limit(Constants.DEFAULT_PAGE_SIZE).ToList();

            var shipmenLiteDtos = fixs.OrderByDescending(item => DateTime.Parse(item.Date)).Select(item => item.ConvertToDto());
            return shipmenLiteDtos;
        }

        public Shipment Get(string id)
        {
            var result = _shipmentLiteRepository.Get(id);
            return result;
        }

        public Shipment CreateBasicShipment()
        {
            Shipment shipment = GenerateShipmentFromTemplate();
            _shipmentLiteRepository.Create(shipment);

            return shipment;
        }

        private Shipment GenerateShipmentFromTemplate()
        {
            var userEmail = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email).Value;

            var shipment = new Shipment
            {
                Date = DateTime.Now.ToString("yyyy'-'MM'-'dd"),
                CompanyName = "NowaNazwa",
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

        public void Update(Shipment shipment) {
            var userEmail = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email).Value;

            if (shipment.Info == null) shipment.Info = new BasicInfo();

            shipment.Info.DateModified = DateTime.UtcNow;
            shipment.Info.ModifiedBy = userEmail;
            _shipmentLiteRepository.Update(shipment);
        }

        public void Remove(string shipmentId) =>
             _shipmentLiteRepository.Remove(shipmentId);
    }
}