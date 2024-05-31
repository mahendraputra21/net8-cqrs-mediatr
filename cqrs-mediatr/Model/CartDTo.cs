using cqrs_mediatr.Models;

namespace cqrs_mediatr.Model
{
    public record CartDto(Guid? Id, List<CartItemDto> Items, decimal TotalPrice);
    public record CartItemDto(Guid? ProductId, string ProductName, string ProductDescription, int Quantity);

}
