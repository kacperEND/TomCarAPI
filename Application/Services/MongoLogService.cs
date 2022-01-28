using Application.Interfaces;
using Domain.Interfaces;
using Domain.Models.MongoDB;
using System;

namespace Application.Services
{
    public class MongoLogService : ILogService
    {
        private readonly IMongoRepository<Log> _logRepository;

        public MongoLogService(IMongoRepository<Log> logRepository)
        {
            _logRepository = logRepository;
        }

        public void Error(string errorMessage, string user, string description, string code = null)
        {
            try
            {
                _logRepository.Create(new Log
                {
                    Message = errorMessage,
                    ErrorCode = code,
                    User = user,
                    Description = description.Substring(0, 150),
                    DateCreated = DateTime.Now
                });
            }
            catch
            {
            }
        }
    }
}