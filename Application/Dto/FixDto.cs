using Domain.Models;
using System;
using System.Collections.Generic;

namespace Application.Dto
{
    public class FixDto
    {
        public int Id { get; set; }
        public int? FixOrderId { get; set; }

        public IList<ElementDto> Elements { get; set; }
    }

    public static class FixDtoExtension
    {
        public static void CopyFromDto(this Fix fix, FixDto fixDto)
        {
            fix.Id = fixDto.Id;
            fix.FixOrderId = fixDto.FixOrderId;
        }

        public static FixDto ConvertToDto(this Fix fix, bool includeDetails = false)
        {
            var elemetsDto = new List<ElementDto>();
            if (includeDetails)
            {
                foreach (var element in fix.Elements)
                {
                    var dto = element.ConvertToDto();
                    elemetsDto.Add(dto);
                }
            }

            var fixDto = new FixDto();
            fixDto.Id = fix.Id;
            fixDto.FixOrderId = fix.FixOrderId;
            fixDto.Elements = elemetsDto;

            return fixDto;
        }
    }
}