using Mediator;

namespace cqrs_mediatr.Features.Products.Notifications
{
    public class StockAssignedHandler(ILogger<StockAssignedHandler> logger) : INotificationHandler<ProductCreatedNotification>
    {
        public ValueTask Handle(ProductCreatedNotification notification, CancellationToken cancellationToken)
        {
            logger.LogInformation($"handling notification for product creation with id : {notification.Id}. assigning stocks.");
            return default;
        }
    }
}
