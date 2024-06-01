namespace cqrs_mediatr.Models
{
    public record ProductDto(Guid Id,
                             string Name,
                             string Description,
                             decimal Price,
                             bool IsDeleted);
}
