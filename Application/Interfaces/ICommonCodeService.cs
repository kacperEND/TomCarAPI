using Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace Application.Interfaces
{
    public interface ICommonCodeService
    {
        CommonCode Get(string parentCode, string code = null);

        CommonCode GetOrCreate(string parentCode, string code = null);

        IQueryable<CommonCode> Query(string parentCode, string code = null);
    }
}