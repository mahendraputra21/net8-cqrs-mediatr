using Mediator;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace DewaEShop.Infrastructure.Behaviors
{
    public class RequestResponseLoggingBehavior<TMessage, TResponse>(ILogger<RequestResponseLoggingBehavior<TMessage, TResponse>> logger)
    : IPipelineBehavior<TMessage, TResponse>
    where TMessage : IMessage
    {
        public async ValueTask<TResponse> Handle(TMessage message, CancellationToken cancellationToken, MessageHandlerDelegate<TMessage, TResponse> next)
        {
            var correlationId = Guid.NewGuid();

            // Request Logging
            // Serialize the request
            var requestJson = JsonSerializer.Serialize(message);
            // Log the serialized request
            logger.LogInformation("Handling request {CorrelationID}: {Request}", correlationId, requestJson);

            // Response logging
            var response = await next(message, cancellationToken);
            // Serialize the request
            var responseJson = JsonSerializer.Serialize(response);
            // Log the serialized request
            logger.LogInformation("Response for {Correlation}: {Response}", correlationId, responseJson);

            // Return response
            return response;
        }
    }
}
