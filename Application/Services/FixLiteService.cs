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
    public class FixLiteService
    {
        private readonly IMongoCollection<FixLite> _Collection;

        public FixLiteService(IMongoDBDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var _database = client.GetDatabase(settings.DatabaseName);

            _Collection = _database.GetCollection<FixLite>(settings.CollectionName);
        }

        public List<FixLite> Get() =>
            _Collection.Find(FixLite => true).ToList();

        public IEnumerable<FixLiteDto> Find(string searchterm = "")
        {
            searchterm = searchterm ?? "";
            var filter = Builders<FixLite>.Filter.Where(x => x.CompanyName.ToUpper().Contains(searchterm.ToUpper()));

            var fixs = _Collection.Find(filter).Limit(Constants.DEFAULT_PAGE_SIZE).ToList();

            var result = fixs.OrderBy(item => DateTime.Parse(item.Date)).Select(item => item.ConvertToDto());

            return result;
        }

        public FixLite Get(string id) =>
            _Collection.Find<FixLite>(FixLite => FixLite.Id == id).FirstOrDefault();

        public FixLite Create()
        {
            FixLite newFix = Template();
            _Collection.InsertOne(newFix);
            return newFix;
        }

        private FixLite Template()
        {
            return new FixLite
            {
                CompanyName = "{{Nazwa}}",
                Date = DateTime.Now.ToString("dd'.'MM'.'yyyy"),
                _PT_Percent = 98,
                _PD_Percent = 98,
                _RH_Percent = 88
            }; ;
        }

        public void Update(string id, FixLite FixLiteIn) =>
            _Collection.ReplaceOne(FixLite => FixLite.Id == id, FixLiteIn);

        public void Remove(FixLite FixLiteIn) =>
            _Collection.DeleteOne(FixLite => FixLite.Id == FixLiteIn.Id);

        public void Remove(string id) =>
            _Collection.DeleteOne(FixLite => FixLite.Id == id);
    }
}