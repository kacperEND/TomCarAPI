using Application.Dto;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommonCodesController : ControllerBase
    {
        private readonly ICommonCodeService _commonCodeService;

        public CommonCodesController(ICommonCodeService commonCodeService)
        {
            _commonCodeService = commonCodeService;
        }

        [HttpGet("Get")]
        public IActionResult Get(string parentCode = null, string searchCode = null, int? pageNo = 1, int? pageSize = 20, bool? searchName = false)
        {
            var itemsToSkip = (pageNo - 1) * pageSize;
            var commonCodes = this._commonCodeService.Query(parentCode);

            if (string.IsNullOrEmpty(parentCode))
            {
                commonCodes = commonCodes.Where(item => item.ParentId == null);
            }

            if (!string.IsNullOrEmpty(searchCode) && !searchName.Value)
            {
                var sanatizedSearchCode = searchCode.Trim().ToUpper();
                commonCodes = commonCodes.Where(item => item.Code.ToUpper().Contains(sanatizedSearchCode));
            }

            if (!string.IsNullOrWhiteSpace(searchCode) && searchName.Value)
            {
                var sanatizedSearchCode = searchCode.Trim().ToUpper();
                commonCodes = commonCodes.Where(item => item.Name.ToUpper().Contains(sanatizedSearchCode));
            }

            var totalCount = commonCodes.Count();

            commonCodes = commonCodes.OrderBy(code => code.Id).Skip(itemsToSkip.Value).Take(pageSize.Value);

            var result = new CollectionResult<CommonCodeDto>();
            result.Items = commonCodes.ToList().Select(code => code.ConvertToDto());
            result.TotalCount = totalCount;
            return Ok(result);
        }

        [HttpGet("CommonCode")]
        public IActionResult CommonCode(string parentCode, string code = null)
        {
            var commonCode = this._commonCodeService.Get(parentCode, code);
            var commonCodeDto = commonCode.ConvertToDto();
            return Ok(commonCodeDto);
        }
    }
}