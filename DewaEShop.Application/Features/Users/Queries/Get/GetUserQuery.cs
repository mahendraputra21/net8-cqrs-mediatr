using DewaEShop.Domain.User;
using Mediator;

namespace DewaEShop.Application.Features.Users.Queries.Get
{
    public record GetUserQuery(string userId) : IRequest<User?>;
}
