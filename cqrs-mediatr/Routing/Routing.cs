using cqrs_mediatr.Features.Carts.Commands.Create;
using cqrs_mediatr.Features.Products.Commands.Create;
using cqrs_mediatr.Features.Products.Commands.Delete;
using cqrs_mediatr.Features.Products.Commands.Update;
using cqrs_mediatr.Features.Products.Notifications;
using cqrs_mediatr.Features.Products.Queries.Get;
using cqrs_mediatr.Features.Products.Queries.List;
using cqrs_mediatr.Models;
using FluentValidation;
using MediatR;

namespace cqrs_mediatr.Routing
{
    public static class Routing
    {
        public static void MapRoutes(IApplicationBuilder app)
        {
            var endpointBuilder = app.UseEndpoints(endpoints =>
            {
                #region Product Endpoints
                
                // Get Product by Id
                endpoints.MapGet("/product/{id:guid}", async (Guid id, ISender mediatr) =>
                {
                    var product = await mediatr.Send(new GetProductQuery(id));
                    if (product == null) return Results.NotFound(new ApiResponseDto<object>(false, "Product not found"));

                    var result = new ApiResponseDto<object>(
                                            true,
                                            "Product retrieved successfully",
                                            product);

                    return Results.Ok(result);
                });

                // Get Product list
                endpoints.MapGet("/products", async (ISender mediatr) =>
                {
                    var products = await mediatr.Send(new ListProductsQuery());

                    var result = new ApiResponseDto<object>(
                                            true,
                                            "Products retrieved successfully",
                                            products);

                    return Results.Ok(result);
                });

                // Add new Product
                endpoints.MapPost("/product", async (
                    CreateProductCommand command,
                    IMediator mediatr,
                    IValidator<CreateProductCommand> validator) =>
                {
                    var validationResult = await validator.ValidateAsync(command);

                    if (!validationResult.IsValid)
                        return Results.ValidationProblem(validationResult.ToDictionary());

                    var productId = await mediatr.Send(command);
                    if (Guid.Empty == productId) 
                        return Results.BadRequest(new ApiResponseDto<object>(false, "Failed to create product"));

                    await mediatr.Publish(new ProductCreatedNotification(productId));

                    var result = new ApiResponseDto<object>(
                                                true,
                                                "Product created successfully",
                                                new { id = productId });

                    return Results.Created($"/products/{productId}", result);
                });

                // Update Existing product by Id
                endpoints.MapPut("/product", async (UpdateProductCommand command,
                                                     ISender mediatr,
                                                     IValidator<UpdateProductCommand> validator) =>
                {

                    var validationResult = await validator.ValidateAsync(command);

                    if (!validationResult.IsValid)
                        return Results.ValidationProblem(validationResult.ToDictionary());

                    var productId = await mediatr.Send(command);
                    if (Guid.Empty == productId) 
                        return Results.BadRequest(new ApiResponseDto<object>(false, "Failed to update product"));

                    var result = new ApiResponseDto<object>(true,
                                            "Product updated successfully",
                                            new { id = productId });

                    return Results.Ok(result);
                });

                // Delete Existing product by Id
                endpoints.MapDelete("/product/{id:guid}", async (Guid id, ISender mediatr) =>
                {
                    await mediatr.Send(new DeleteProductCommand(id));
                    return Results.NoContent();
                });
                #endregion

                #region Cart Endpoints

                // Add Product to cart
                endpoints.MapPost("/cart", async (
                        CreateCartCommand command,
                        ISender mediatr) =>
                {

                    var cartDto = await mediatr.Send(command);
                   
                    var result = new ApiResponseDto<object>(
                                                true,
                                                "Product Added successfully on Cart",
                                                new { cartDto });

                    return Results.Created($"/cart/{cartDto.Id}", result);
                });
                #endregion

            });
        }
    }
}
