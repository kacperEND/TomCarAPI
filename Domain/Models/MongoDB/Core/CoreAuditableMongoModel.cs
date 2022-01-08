using Domain.Interfaces;
using Domain.Models.MongoDB.Core;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Models.MongoDB
{
    public abstract class CoreAuditableMongoModel : ICoreAuditableMongoModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public BasicInfo Info { get; set; }
    }
}