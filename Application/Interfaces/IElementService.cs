using Application.Dto;

namespace Application.Interfaces
{
    public interface IElementService
    {
        ElementDto Create(ElementDto elementDto);
    }
}