using cqrs_mediatr.Domain;
using Mediator;

namespace cqrs_mediatr.Features.Users.Queries.Get
{
    public record GetUserQuery(string userId) : IRequest<User?>;
}
