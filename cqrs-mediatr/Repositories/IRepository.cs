namespace cqrs_mediatr.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<T> InsertAsync(T entity, CancellationToken cancellationToken);
        Task UpdateAsync(T entity, CancellationToken cancellationToken);
        Task DeleteAsync(T entity, CancellationToken cancellationToken);
    }
}
