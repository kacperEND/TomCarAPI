using Application.Dto;
using Domain.Models;
using System.Collections.Generic;

namespace Application.Interfaces
{
    public interface IFixService
    {
        void AddEditElements(int? fixId, IList<ElementDto> elementsDto);

        Fix Create(FixDto fixDto);

        string GenerateFixReport(int? fixId);
    }
}