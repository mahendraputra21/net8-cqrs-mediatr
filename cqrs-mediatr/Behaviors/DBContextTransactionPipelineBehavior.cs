using cqrs_mediatr.Persistence;
using MediatR;

namespace cqrs_mediatr.Behaviors
{
    /// <summary>
    /// Adds transaction to the processing pipeline
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public class DBContextTransactionPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly AppDbContext _appDbContext;

        public DBContextTransactionPipelineBehavior(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
           TResponse? result = default;
            try
            {
                _appDbContext.Database.BeginTransaction();
                result = await next();
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
