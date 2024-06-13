using FluentValidation;
using Mediator;

namespace DewaEShop.Application.Behaviors
{
    public class ValidationBehavior<TMessage, TResponse>(IEnumerable<IValidator<TMessage>> validators)
    : IPipelineBehavior<TMessage, TResponse>
    where TMessage : IMessage
    {
        public async ValueTask<TResponse> Handle(TMessage message, CancellationToken cancellationToken, MessageHandlerDelegate<TMessage, TResponse> next)
        {
            ArgumentNullException.ThrowIfNull(next);

            if (validators.Any())
            {
                var context = new ValidationContext<TMessage>(message);

                var validationResults = await Task.WhenAll(
                    validators.Select(v =>
                        v.ValidateAsync(context, cancellationToken))).ConfigureAwait(false);

                var failures = validationResults
                    .Where(r => r.Errors.Count > 0)
                    .SelectMany(r => r.Errors)
                    .ToList();

                if (failures.Count > 0)
                    throw new ValidationException(failures);
            }
            return await next(message, cancellationToken).ConfigureAwait(false);
        }
    }
}
