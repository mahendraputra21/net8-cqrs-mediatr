using Mediator;

namespace cqrs_mediatr.Features.Products.Notifications
{
    public record ProductCreatedNotification(Guid Id) : INotification;
}
