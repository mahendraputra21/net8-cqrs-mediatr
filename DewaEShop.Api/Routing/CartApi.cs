using DewaEShop.Application.Features.Carts.Commands.Create;
using DewaEShop.Contract;
using Mediator;

namespace DewaEShop.Routing
{
    public static class CartApi
    {
        public static IEndpointRouteBuilder MapCartApi(this IEndpointRouteBuilder endpoints)
        {
            var baseAPI = new BaseApi(endpoints);
            var cartsRoutes = baseAPI.CreateRouteGroup(endpoints, "carts").MapToApiVersion(1);

            cartsRoutes.MapPost("/", async (CreateCartRequest request, ISender mediatr) =>
            {
                var command = new CreateCartCommand(request.CartId, request.ProductId, request.Quantity);
                var cartDto = await mediatr.Send(command);

                var result = new ApiResponseDto<object>(true, "Product Added successfully to Cart", new { cartDto });
                return Results.Created($"/cart/{cartDto.Id}", result);
            });
            //.RequireAuthorization();

            return endpoints;
        }
    }
}
