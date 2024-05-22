using cqrs_mediatr.Features.Products.Commands.Create;
using cqrs_mediatr.Features.Products.Commands.Delete;
using cqrs_mediatr.Features.Products.Commands.Update;
using cqrs_mediatr.Features.Products.Notifications;
using cqrs_mediatr.Features.Products.Queries.Get;
using cqrs_mediatr.Features.Products.Queries.List;
using cqrs_mediatr.Persistence;
using cqrs_mediatr.Routing;
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

app.UseHttpsRedirection();
app.UseRouting();
// Register Routing endpoints
Routing.MapRoutes(app);

app.Run();


