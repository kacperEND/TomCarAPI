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
                    Type = LogType.Error,
                    User = user,
                    Description = description.Substring(0, 150),
                    DateCreated = DateTime.Now
                });
            }
            catch
            {
            }
        }

        public void Info(string message, string user, string description)
        {
            try
            {
                _logRepository.Create(new Log
                {
                    Message = message,
                    Type = LogType.Info,
                    User = user,
                    Description = description,
                    DateCreated = DateTime.Now
                });
            }
            catch
            {
            }
        }
    }
}