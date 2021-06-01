using Application.Dto;
using Domain.Models;
using System.Collections.Generic;

namespace Application.Interfaces
{
    public interface IElementService
    {
        ElementDto Create(ElementDto elementDto);
    }
}