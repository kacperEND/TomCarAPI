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

        void LoadCommonCodesIntoDb();

        IQueryable<CommonCode> Query(string parentCode, List<string> codes);

        CommonCode CreateCommonCodeFromName(string name, int? parentId = null);

        ILookup<int, CommonCode> GetTranslationLookup(string code);

        string GetTranslationByCode(string parentCode, string codeToTranslate, ILookup<int, CommonCode> translationLookup);

        string GetTranslationFromLookup(int id, ILookup<int, CommonCode> translationLookup);

        CommonCode createIfNotExist(string name, string code, string description, string parentCode = null);
    }
}