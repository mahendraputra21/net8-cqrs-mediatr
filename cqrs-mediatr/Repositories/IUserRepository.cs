using cqrs_mediatr.Domain;
using cqrs_mediatr.Persistence;

namespace cqrs_mediatr.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetCurrentUserIdentity(string userId);
    }

    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(AppDbContext db) : base(db)
        {
        }

        public async Task<User> GetCurrentUserIdentity(string userId)
        {
            return await db.Users.FindAsync(userId);
        }
    }
}
