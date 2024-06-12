namespace DewaEShop.Contract
{
    public record ProductDto(Guid Id,
                             string Name,
                             string Description,
                             decimal Price,
                             bool IsDeleted);
}
