using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.MongoDB
{
    public class Log : CoreAuditableMongoModel
    {
        public string Message { get; set; }
        public string Description { get; set; }
        public string ErrorCode { get; set; }
        public string User { get; set; }
        public DateTime DateCreated { get; set; }
    }
}