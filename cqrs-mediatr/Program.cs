using cqrs_mediatr.Behaviors;
using cqrs_mediatr.Exceptions;
using cqrs_mediatr.Features.Carts.Commands.Create;
using cqrs_mediatr.Features.Products.Commands.Create;
using cqrs_mediatr.Features.Products.Commands.Delete;
using cqrs_mediatr.Features.Products.Commands.Update;
using cqrs_mediatr.Features.Products.Queries.Get;
using cqrs_mediatr.Persistence;
using cqrs_mediatr.Repositories;
using cqrs_mediatr.Repositories.Configuration;
using cqrs_mediatr.Routing;
using FluentValidation;
using Mediator;
using System.Diagnostics;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registering DB Context
builder.Services.AddDbContext<AppDbContext>();

// Registering MediatR add pipeline behaviour

builder.Services.AddMediator(options =>
{
    options.ServiceLifetime = ServiceLifetime.Scoped;
});

builder.Services.AddScoped<IGuidGenerator, DefaultGuidGenerator>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();

// Register behaviors
builder.Services.AddScoped(typeof(RequestResponseLoggingBehavior<,>));
builder.Services.AddScoped(typeof(ValidationBehavior<,>));
builder.Services.AddScoped(typeof(DBContextTransactionPipelineBehavior<,>));


builder.Services.AddScoped<CreateCartCommand>();

builder.Services.AddScoped<CreateProductCommand>(provider =>
{
    var name = "example";
    var description = "example";
    var price = 0;
    return new CreateProductCommand(name, description, price);
});
builder.Services.AddScoped<UpdateProductCommand>(provider =>
{
    var id = Guid.NewGuid(); // Replace with actual logic to get Guid
    var name = "example";
    var description = "example";
    var price = 0;
    return new UpdateProductCommand(id, name, description, price);
});

builder.Services.AddScoped<DeleteProductCommand>(provider =>
{
    var guid = Guid.NewGuid(); // Replace with actual logic to get Guid
    return new DeleteProductCommand(guid);
});

builder.Services.AddScoped<GetProductQuery>(provider =>
{
    var guid = Guid.NewGuid(); // Replace with actual logic to get Guid
    return new GetProductQuery(guid);
});


builder.Services.AddScoped<GetProductQueryHandler, GetProductQueryHandler>();
builder.Services.AddScoped<CreateCartCommandHandler, CreateCartCommandHandler>();
builder.Services.AddScoped<CreateProductCommandHandler, CreateProductCommandHandler>();
builder.Services.AddScoped<UpdateProductCommandHandler, UpdateProductCommandHandler>();
builder.Services.AddScoped<DeleteProductCommandHandler, DeleteProductCommandHandler>();

builder.Services.AddScoped<CreateProductCommandValidator, CreateProductCommandValidator>();
builder.Services.AddScoped<UpdateProductCommandValidator, UpdateProductCommandValidator>();


// Registering Automapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// registering Fluent Validator
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

// Registering Global Exception handler
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

// Registering Problem Details
builder.Services.AddProblemDetails( options =>
{
    options.CustomizeProblemDetails = (context) => 
    {
        if (!context.ProblemDetails.Extensions.ContainsKey("traceId"))
        {
            string? traceId = Activity.Current?.Id ?? context.HttpContext.TraceIdentifier;
            context.ProblemDetails.Extensions.Add(new KeyValuePair<string, object?>("traceId", traceId));
        }
    };
});

// Registeering DI
//builder.Services.AddRepositories();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseExceptionHandler();

// Register Routing endpoints
Routing.MapRoutes(app);

app.Run();


