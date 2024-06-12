using Mediator;

namespace DewaEShop.Application.Features.Products.Notifications
{
    public record ProductCreatedNotification(Guid Id) : INotification;
}
