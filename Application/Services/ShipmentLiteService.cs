using Domain.Interfaces;
using Domain.Models.MongoDB;
using Infrastructure.Data.MongoDB;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using WebAPI.DtoModels;

namespace Application.Services
{
    public class ShipmentLiteService
    {
        private readonly IMongoRepository<Shipment> _shipmentLiteRepository;

        public ShipmentLiteService(IMongoRepository<Shipment> shipmentLiteRepository)
        {
            _shipmentLiteRepository = shipmentLiteRepository;
        }

        public IEnumerable<ShipmenLiteDto> Find(string searchterm = "")
        {
            searchterm = searchterm ?? "";
            var filter = Builders<Shipment>.Filter.Where(x => x.CompanyName.ToUpper().Contains(searchterm.ToUpper()));

            var fixs = _shipmentLiteRepository.Collection.Find(filter).Limit(Constants.DEFAULT_PAGE_SIZE).ToList();

            var shipmenLiteDtos = fixs.OrderByDescending(item => DateTime.Parse(item.Date)).Select(item => item.ConvertToDto());
            return shipmenLiteDtos;
        }

        public Shipment Get(string id)
        {
            var result = _shipmentLiteRepository.Get(id);
            return result;
        }

        public Shipment Create(Shipment shipmentLite)
        {
            shipmentLite = null; //TODO
            Shipment newFix = shipmentLite ?? GenerateTemplateShipment();
            _shipmentLiteRepository.Create(newFix);

            return newFix;
        }

        private Shipment GenerateTemplateShipment()
        {
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
            };

            return shipment;
        }

        public void Update(Shipment shipment) =>
            _shipmentLiteRepository.Update(shipment);

        public void Remove(string shipmentId) =>
             _shipmentLiteRepository.Remove(shipmentId);
    }
}