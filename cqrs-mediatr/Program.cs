using cqrs_mediatr.Features.Products.Commands.Create;
using cqrs_mediatr.Features.Products.Commands.Delete;
using cqrs_mediatr.Features.Products.Commands.Update;
using cqrs_mediatr.Features.Products.Notifications;
using cqrs_mediatr.Features.Products.Queries.Get;
using cqrs_mediatr.Features.Products.Queries.List;
using cqrs_mediatr.Persistence;
using MediatR;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Registering DB Context
builder.Services.AddDbContext<AppDbContext>();
// Registering MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/products/{id:guid}", async (Guid id, ISender mediatr) =>
{
    var product = await mediatr.Send(new GetProductQuery(id));
    if (product == null) return Results.NotFound();
    return Results.Ok(product);
});

app.MapGet("/products", async (ISender mediatr) =>
{
    var products = await mediatr.Send(new ListProductsQuery());
    return Results.Ok(products);
});

app.MapPost("/products", async (CreateProductCommand command, IMediator mediatr) =>
{
    var productId = await mediatr.Send(command);
    if(Guid.Empty == productId) return Results.BadRequest();
    await mediatr.Publish(new ProductCreatedNotification(productId));
    return Results.Created($"/products/{productId}", new { id = productId});
});

app.MapPut("/products", async (UpdateProductCommand command, ISender mediatr) =>
{
    var productId = await mediatr.Send(command);
    if (Guid.Empty == productId) return Results.BadRequest();
    return Results.Ok($"Update Product on Id: {productId} successfully");
});

app.MapDelete("/products/{id:guid}", async (Guid id, ISender mediatr) =>
{
    await mediatr.Send(new DeleteProductCommand(id));
    return Results.NoContent();
});

app.UseHttpsRedirection();
app.Run();


