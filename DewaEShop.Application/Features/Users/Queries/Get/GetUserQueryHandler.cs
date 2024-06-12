using DewaEShop.Domain.User;
using DewaEShop.Infrastructure.Repositories;
using Mediator;

namespace DewaEShop.Application.Features.Users.Queries.Get
{
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, User?>
    {
        private readonly IUserRepository _userRepository;

        public GetUserQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async ValueTask<User?> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetCurrentUserIdentity(request.userId);
            if (user == null) return null;
            return user;
        }
    }
}
