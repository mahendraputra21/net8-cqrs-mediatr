using DewaEShop.Application.Features.Users.Queries.Get;
using DewaEShop.Contract;
using Mediator;
using System.Security.Claims;

namespace DewaEShop.Routing
{
    public static class UsersApi
    {
        public static IEndpointRouteBuilder MapUsersApi(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("user/current", async (ISender mediator, ClaimsPrincipal claim) =>
            {
                var userId = claim.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                if (userId == null)
                {
                    var errorResponse = new ApiResponseDto<object>(false, "User Claim not found");
                    return Results.NotFound(errorResponse);
                }

                var query = new GetUserQuery(userId);
                var user = await mediator.Send(query);

                if (user == null)
                {
                    var errorResponse = new ApiResponseDto<object>(false, "User not found");
                    return Results.NotFound(errorResponse);
                }

                var result = new ApiResponseDto<object>(true, "User retrieved successfully", user);
                return Results.Ok(result);
            })
            .RequireAuthorization();

            return endpoints;
        }
    }
}
