using Domain.Models;

namespace Application.Dto
{
    public class ElementDto
    {
        public int Id { get; set; }
        public int? FixId { get; set; }
        public decimal? Price { get; set; }
        public double? NetWeight { get; set; }
        public int? NameCodeId { get; set; }
        public string NameCodeCode { get; set; }
        public string NameCodeName { get; set; }
        public double? Percent { get; set; }
        public double? Result { get; set; }
    }

    public static class ElementDtoExtension
    {
        public static void CopyFromDto(this Element model, ElementDto dto)
        {
            //model.Id = dto.Id;
            model.Price = dto.Price;
            model.Percent = dto.Percent;
            model.FixId = dto.FixId;
            model.Result = dto.Result;
            model.NetWeight = dto.NetWeight;
            model.CommonCodeNameId = dto.NameCodeId;
        }

        public static ElementDto ConvertToDto(this Element model)
        {
            var dto = new ElementDto();
            dto.Id = model.Id;
            dto.Price = model.Price;
            dto.Percent = model.Percent;
            dto.Result = model.Result;
            dto.FixId = model.FixId;
            dto.NetWeight = model.NetWeight;
            dto.NameCodeId = model.CommonCodeNameId.HasValue ? model.CommonCodeNameId : null;
            dto.NameCodeCode = model.CommonCodeName?.Code;
            dto.NameCodeName = model.CommonCodeName?.Name;

            return dto;
        }
    }
}