using cqrs_mediatr.Domain;
using cqrs_mediatr.Repositories;
using Mediator;

namespace cqrs_mediatr.Features.Users.Queries.Get
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
