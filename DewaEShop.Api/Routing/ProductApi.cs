using DewaEShop.Application.Features.Products.Commands.Create;
using DewaEShop.Application.Features.Products.Commands.Delete;
using DewaEShop.Application.Features.Products.Commands.Update;
using DewaEShop.Application.Features.Products.Notifications;
using DewaEShop.Application.Features.Products.Queries.Get;
using DewaEShop.Application.Features.Products.Queries.List;
using DewaEShop.Contract;
using FluentValidation;
using Mediator;
using System.Security.Claims;

namespace DewaEShop.Routing
{
    public static class ProductApi 
    {
        public static IEndpointRouteBuilder MapProductApi(this IEndpointRouteBuilder endpoints)
        {
            var baseAPI = new BaseApi(endpoints);
            var productRoutes = baseAPI.CreateRouteGroup(endpoints, "products").MapToApiVersion(1);

            productRoutes.MapGet("{id:guid}", async (Guid id, ISender mediatr) =>
            {
                var query = new GetProductQuery(id);
                var product = await mediatr.Send(query);

                if (product == null)
                {
                    var errorResponse = new ApiResponseDto<object>(false, "Product not found");
                    return Results.NotFound(errorResponse);
                }

                var result = new ApiResponseDto<object>(true, "Product retrieved successfully", product);
                return Results.Ok(result);
            });
            //.RequireAuthorization();

            productRoutes.MapGet("/", async (ISender mediatr, ClaimsPrincipal claim) =>
            {
                var query = new ListProductsQuery();
                var products = await mediatr.Send(query);

                var result = new ApiResponseDto<object>(true, $"Products retrieved successfully accessed by user {claim.Identity?.Name}", products);
                return Results.Ok(result);
            });
            //.RequireAuthorization();

            productRoutes.MapPost("/", async (ProductDto request, IMediator mediatr, IValidator<CreateProductCommand> validator) =>
            {
                var command = new CreateProductCommand(request.Name, request.Description, request.Price);
                var validationResult = await validator.ValidateAsync(command);

                if (!validationResult.IsValid)
                    return Results.ValidationProblem(validationResult.ToDictionary());

                var productId = await mediatr.Send(command);

                if (Guid.Empty == productId)
                {
                    var errorResponse = new ApiResponseDto<object>(false, "Failed to create product");
                    return Results.BadRequest(errorResponse);
                }

                await mediatr.Publish(new ProductCreatedNotification(productId));
                var result = new ApiResponseDto<object>(true, "Product created successfully", new { id = productId });
                return Results.Created($"/products/{productId}", result);
            });
            //.RequireAuthorization();

            productRoutes.MapPut("/", async (ProductDto request, ISender mediatr, IValidator<UpdateProductCommand> validator) =>
            {
                var command = new UpdateProductCommand(request.Id, request.Name, request.Description, request.Price);
                var validationResult = await validator.ValidateAsync(command);

                if (!validationResult.IsValid)
                    return Results.ValidationProblem(validationResult.ToDictionary());

                var productId = await mediatr.Send(command);

                if (Guid.Empty == productId)
                {
                    var errorResponse = new ApiResponseDto<object>(false, "Failed to update product");
                    return Results.BadRequest(errorResponse);
                }

                var result = new ApiResponseDto<object>(true, "Product updated successfully", new { id = productId });
                return Results.Ok(result);
            });
            //.RequireAuthorization();

            productRoutes.MapDelete("{id:guid}", async (Guid id, ISender mediatr) =>
            {
                await mediatr.Send(new DeleteProductCommand(id));
                return Results.NoContent();
            });
            //.RequireAuthorization();

            return endpoints;
        }
    }
}
