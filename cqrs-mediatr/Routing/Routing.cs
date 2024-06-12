using cqrs_mediatr.Domain;
using cqrs_mediatr.Features.Carts.Commands.Create;
using cqrs_mediatr.Features.Products.Commands.Create;
using cqrs_mediatr.Features.Products.Commands.Delete;
using cqrs_mediatr.Features.Products.Commands.Update;
using cqrs_mediatr.Features.Products.Notifications;
using cqrs_mediatr.Features.Products.Queries.Get;
using cqrs_mediatr.Features.Products.Queries.List;
using cqrs_mediatr.Features.Users.Queries.Get;
using cqrs_mediatr.Model;
using cqrs_mediatr.Models;
using FluentValidation;
using Mediator;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using SendGrid.Lib;
using System.Security.Claims;

namespace cqrs_mediatr.Routing
{
    public static class Routing
    {
        public static void MapRoutes(IApplicationBuilder app)
        {
            var endpointBuilder = app.UseEndpoints(endpoints =>
            {
                #region PRODUCTS Endpoints
                // Get Product by Id
                endpoints.MapGet("/product/{id:guid}", async (Guid id, ISender mediatr) =>
                {
                    var query = new GetProductQuery(id);

                    var product = await mediatr.Send(query);

                    if (product == null)
                    {
                        var errorResponse = new ApiResponseDto<object>(false, "Product not found");
                        return Results.NotFound(errorResponse);
                    }

                    var result = new ApiResponseDto<object>(
                                            true,
                                            "Product retrieved successfully",
                                            product);

                    return Results.Ok(result);
                })
                .RequireAuthorization();

                // Get Product list
                endpoints.MapGet("/products", async (ISender mediatr, ClaimsPrincipal claim) =>
                {
                    var query = new ListProductsQuery();

                    var products = await mediatr.Send(query);

                    var result = new ApiResponseDto<object>(
                                            true,
                                            $"Products retrieved successfully accesed by user {claim.Identity?.Name}",
                                            products);

                    return Results.Ok(result);
                })
                .RequireAuthorization();

                // Add new Product
                endpoints.MapPost("/product", async (
                    ProductDto request,
                    IMediator mediatr,
                    IValidator<CreateProductCommand> validator) =>
                {
                    var command = new CreateProductCommand(
                        request.Name,
                        request.Description,
                        request.Price);

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

                    var result = new ApiResponseDto<object>(
                                                true,
                                                "Product created successfully",
                                                new { id = productId });

                    return Results.Created($"/products/{productId}", result);
                })
                .RequireAuthorization();

                // Update Existing product by Id
                endpoints.MapPut("/product", async (ProductDto request,
                                                     ISender mediatr,
                                                     IValidator<UpdateProductCommand> validator) =>
                {
                    var command = new UpdateProductCommand
                        (request.Id,
                        request.Name,
                        request.Description,
                        request.Price);

                    var validationResult = await validator.ValidateAsync(command);

                    if (!validationResult.IsValid)
                        return Results.ValidationProblem(validationResult.ToDictionary());

                    var productId = await mediatr.Send(command);

                    if (Guid.Empty == productId)
                    {
                        var errorResponse = new ApiResponseDto<object>(false, "Failed to update product");
                        return Results.BadRequest(errorResponse);
                    }

                    var result = new ApiResponseDto<object>(true,
                                            "Product updated successfully",
                                            new { id = productId });

                    return Results.Ok(result);
                })
                .RequireAuthorization();

                // Delete Existing product by Id
                endpoints.MapDelete("/product/{id:guid}", async (Guid id, ISender mediatr) =>
                {
                    await mediatr.Send(new DeleteProductCommand(id));
                    return Results.NoContent();
                })
                .RequireAuthorization();
                #endregion

                #region CART Endpoints
                // Add Product to cart
                endpoints.MapPost("/cart", async (
                        CreateCartRequest request,
                        ISender mediatr) =>
                {
                    var command = new CreateCartCommand(
                        request.CartId,
                        request.ProductId,
                        request.Quatity);

                    var cartDto = await mediatr.Send(command);

                    var result = new ApiResponseDto<object>(
                                                true,
                                                "Product Added successfully on Cart",
                                                new { cartDto });

                    return Results.Created($"/cart/{cartDto.Id}", result);
                })
                .RequireAuthorization();
                #endregion

                #region USERS Endpoints
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

                    if(user == null)
                    {
                        var errorResponse = new ApiResponseDto<object>(false, "User not found");
                        return Results.NotFound(errorResponse);
                    }

                    var result = new ApiResponseDto<object>(
                            true,
                            "User retrieved successfully",
                            user);

                    return Results.Ok(result);
                })
                .RequireAuthorization();
                #endregion

                #region SENDGRID Endpoints
                endpoints.MapPost("/sg-send-template", async (
                    ISendGridEmailSender emailSender,
                    [FromBody] SendEmailRequestDto request) =>
                {

                    await emailSender.SendEmailWithTemplateAsync(
                        request.ToEmail,
                        request.Subject,
                        request.TemplateId
                        );

                    var result = new ApiResponseDto<object>(
                            true,
                            "Template email sent successfully"
                        );

                    return Results.Ok(result);

                });
                #endregion
            });
        }
    }
}
