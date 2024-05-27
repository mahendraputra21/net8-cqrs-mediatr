using cqrs_mediatr.Features.Products.Commands.Create;
using cqrs_mediatr.Features.Products.Commands.Delete;
using cqrs_mediatr.Features.Products.Commands.Update;
using cqrs_mediatr.Features.Products.Notifications;
using cqrs_mediatr.Features.Products.Queries.Get;
using cqrs_mediatr.Features.Products.Queries.List;
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
                endpoints.MapGet("/products/{id:guid}", async (Guid id, ISender mediatr) =>
                {
                    var product = await mediatr.Send(new GetProductQuery(id));
                    if (product == null) return Results.NotFound();
                    return Results.Ok(product);
                });

                endpoints.MapGet("/products", async (ISender mediatr) =>
                {
                    var products = await mediatr.Send(new ListProductsQuery());
                    return Results.Ok(products);
                });

                endpoints.MapPost("/products", async (CreateProductCommand command, IMediator mediatr, IValidator<CreateProductCommand> validator) =>
                {
                    var validationResult = await validator.ValidateAsync(command);

                    if (!validationResult.IsValid)
                        return Results.ValidationProblem(validationResult.ToDictionary());

                    var productId = await mediatr.Send(command);
                    if (Guid.Empty == productId) return Results.BadRequest();
                    await mediatr.Publish(new ProductCreatedNotification(productId));
                    return Results.Created($"/products/{productId}", new { id = productId });
                });

                endpoints.MapPut("/products", async (UpdateProductCommand command, ISender mediatr) =>
                {
                    var productId = await mediatr.Send(command);
                    if (Guid.Empty == productId) return Results.BadRequest();
                    return Results.Ok($"Update Product on Id: {productId} successfully");
                });

                endpoints.MapDelete("/products/{id:guid}", async (Guid id, ISender mediatr) =>
                {
                    await mediatr.Send(new DeleteProductCommand(id));
                    return Results.NoContent();
                });
                #endregion
            });
        }
    }
}
