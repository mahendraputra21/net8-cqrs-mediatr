namespace DewaEShop.Contract
{
    public record CartDto(Guid? Id, List<CartItemDto> Items, decimal TotalPrice);
    public record CartItemDto(Guid? ProductId, string ProductName, string ProductDescription, int Quantity);
    public record CreateCartRequest(Guid CartId, Guid ProductId, int Quatity);

}
