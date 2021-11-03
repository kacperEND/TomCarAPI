using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ILogService
    {
        void Error(string errorMessage, string user, string description, string code = null);
    }
}