using Application.Interfaces;
using Domain.Interfaces;
using Domain.Models.MongoDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class MongoLogService : ILogService
    {
        private readonly IMongoRepository<Log> _repository;

        public MongoLogService(IMongoRepository<Log> logRepo)
        {
            _repository = logRepo;
        }

        public void Error(string errorMessage, string user, string description, string code = null)
        {
            try
            {
                var log = new Log
                {
                    Message = errorMessage,
                    ErrorCode = code,
                    User = user,
                    Description = description.Substring(0, 150),
                    DateCreated = DateTime.Now
                };

                _repository.Create(log);
            }
            catch
            {
                //DO NOTHING
            }
        }
    }
}