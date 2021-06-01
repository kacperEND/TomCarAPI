using Application.Interfaces;
using Domain.Interfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Services
{
    public class CommonCodeService : ICommonCodeService
    {
        private readonly IRepository<CommonCode> _commonCode;

        public CommonCodeService(IRepository<CommonCode> commonCode)
        {
            _commonCode = commonCode;
        }

        public IQueryable<CommonCode> Query(string parentCode = null, string code = null)
        {
            var query = this._commonCode.Table.Where(commonCodeItem => commonCodeItem.IsDeleted == false);

            if (!string.IsNullOrWhiteSpace(parentCode))
            {
                query = query.Where(item => item.Parent != null && item.Parent.Code == parentCode);
            }

            if (!string.IsNullOrWhiteSpace(code))
            {
                query = query.Where(commonCodeItem => commonCodeItem.Code.ToUpper() == code.ToUpper());
            }
            return query;
        }

        public string GetTranslationByCode(string parentCode, string codeToTranslate, ILookup<int, CommonCode> translationLookup)
        {
            if (string.IsNullOrEmpty(parentCode) || string.IsNullOrEmpty(codeToTranslate))
                return codeToTranslate;

            var commonCode = Get(parentCode, codeToTranslate);
            if (commonCode == null)
                return codeToTranslate;

            return GetTranslationFromLookup(commonCode.Id, translationLookup);
        }

        public string GetTranslationFromLookup(int id, ILookup<int, CommonCode> translationLookup)
        {
            if (translationLookup == null)
                return null;

            if (translationLookup.Contains(id) && !string.IsNullOrWhiteSpace(translationLookup[id].Select(item => item.Name).FirstOrDefault()))
                return translationLookup[id].Select(item => item.Name).FirstOrDefault();
            else
                return null;
        }

        public CommonCode Get(string parentCode, string code = null)
        {
            if (string.IsNullOrWhiteSpace(parentCode))
            {
                throw new ArgumentNullException("parentCode");
            }

            if (string.IsNullOrWhiteSpace(code))
            {
                return this._commonCode.Get(commonCodeItem => commonCodeItem.Code.ToUpper() == parentCode.ToUpper());
            }

            return this._commonCode.Get(commonCodeItem => commonCodeItem.Parent != null && commonCodeItem.Code.ToUpper() == code.ToUpper() && commonCodeItem.Parent.Code == parentCode);
        }

        public CommonCode GetOrCreate(string parentCode, string code = null)
        {
            if (string.IsNullOrWhiteSpace(parentCode))
            {
                throw new ArgumentNullException("parentCode");
            }

            var parentCommonCode = this._commonCode.Get(commonCodeItem => commonCodeItem.Code.ToUpper() == parentCode.ToUpper());
            if (parentCommonCode == null)
            {
                parentCommonCode = this.CreateCommonCode(parentCode);
            }

            if (string.IsNullOrWhiteSpace(code))
            {
                return parentCommonCode;
            }

            var childCommonCode = this._commonCode.Get(commonCodeItem => commonCodeItem.Parent != null && commonCodeItem.Code.ToUpper() == code.ToUpper() && commonCodeItem.ParentId == parentCommonCode.Id);
            if (childCommonCode == null)
            {
                childCommonCode = this.CreateCommonCode(code, parentCommonCode.Id);
            }

            return childCommonCode;
        }

        private CommonCode CreateCommonCode(string code, int? parentId = null)
        {
            var newCommonCode = new CommonCode();
            newCommonCode.Name = code;
            newCommonCode.Code = code;
            newCommonCode.ParentId = parentId;
            this._commonCode.Create(newCommonCode);
            this._commonCode.Flush();
            return newCommonCode;
        }

        public void LoadCommonCodesIntoDb()
        {
            throw new NotImplementedException();
        }

        public IQueryable<CommonCode> Query(string parentCode, List<string> codes)
        {
            throw new NotImplementedException();
        }

        public CommonCode CreateCommonCodeFromName(string name, int? parentId = null)
        {
            throw new NotImplementedException();
        }

        public ILookup<int, CommonCode> GetTranslationLookup(string code)
        {
            throw new NotImplementedException();
        }

        public CommonCode createIfNotExist(string name, string code, string description, string parentCode = null)
        {
            throw new NotImplementedException();
        }
    }
}