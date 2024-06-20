using DewaEShop.Application.Features.Carts.Commands.Create;
using DewaEShop.Contract;
using Mediator;

namespace DewaEShop.Routing
{
    public static class CartApi
    {
        public static IEndpointRouteBuilder MapCartApi(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapPost("/cart", async (CreateCartRequest request, ISender mediatr) =>
            {
                var command = new CreateCartCommand(request.CartId, request.ProductId, request.Quantity);
                var cartDto = await mediatr.Send(command);

                var result = new ApiResponseDto<object>(true, "Product Added successfully to Cart", new { cartDto });
                return Results.Created($"/cart/{cartDto.Id}", result);
            })
            .RequireAuthorization();

            return endpoints;
        }
    }
}
