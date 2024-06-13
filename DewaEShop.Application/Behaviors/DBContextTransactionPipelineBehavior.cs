using DewaEShop.Infrastructure.Persistence;
using Mediator;

namespace DewaEShop.Application.Behaviors
{
    /// <summary>
    /// Adds transaction to the processing pipeline
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public class DBContextTransactionPipelineBehavior<TMessage, TResponse> : IPipelineBehavior<TMessage, TResponse>
    where TMessage : IMessage // Constrained to IMessage, or constrain to IBaseCommand or any custom interface you've implemented
    {
        private readonly AppDbContext _appDbContext;

        public DBContextTransactionPipelineBehavior(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async ValueTask<TResponse> Handle(TMessage message, CancellationToken cancellationToken, MessageHandlerDelegate<TMessage, TResponse> next)
        {
            TResponse? result = default;
            try
            {
                _appDbContext.Database.BeginTransaction();
                result = await next(message, cancellationToken);
                _appDbContext.Database.CommitTransaction();
            }
            catch (Exception)
            {
                _appDbContext.Database.RollbackTransaction();
                throw;
            }

            return result;
        }
    }
}
