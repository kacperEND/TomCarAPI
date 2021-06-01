using Domain.Models;

namespace Application.Dto
{
    public class CommonCodeDto
    {
        public CommonCodeDto()
        {
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? ParentId { get; set; }
        public bool IsDeleted { get; set; }
        public bool? AllowUserDefinedCodes { get; set; }
        public string ExternalReference { get; set; }
    }

    public static class CommonCodeDtoExtension
    {
        public static void CopyFromDto(this CommonCode code, CommonCodeDto codeDto)
        {
            code.Name = codeDto.Name;
            code.Code = codeDto.Code;
            code.Description = codeDto.Description;
            code.ParentId = codeDto.ParentId;
            code.IsDeleted = codeDto.IsDeleted;
            code.ExternalReference = codeDto.ExternalReference;
        }

        public static CommonCodeDto ConvertToDto(this CommonCode commonCode)
        {
            var dto = new CommonCodeDto();
            dto.Id = commonCode.Id;
            dto.Name = commonCode.Name;
            dto.Code = commonCode.Code;
            dto.Description = commonCode.Description;
            if (commonCode.ParentId != null)
            {
                dto.ParentId = commonCode.ParentId;
            }
            dto.IsDeleted = commonCode.IsDeleted;
            dto.ExternalReference = commonCode.ExternalReference;
            return dto;
        }
    }
}