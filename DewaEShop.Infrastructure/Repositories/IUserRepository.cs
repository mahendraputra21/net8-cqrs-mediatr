using DewaEShop.Domain.User;
using DewaEShop.Infrastructure.Persistence;

namespace DewaEShop.Infrastructure.Repositories
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
