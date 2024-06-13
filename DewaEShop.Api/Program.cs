using DewaEShop.Application.Configuration;
using DewaEShop.Configuration;
using DewaEShop.Domain.User;
using DewaEShop.Infrastructure.Configuration;
using DewaEShop.Routing;
using DewaEShop.SendGrid.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Registeering DI

builder.Services.AddInfrastructure();
builder.Services.AddApplication();
builder.Services.AddPresentation();
builder.Services.AddSendGridDependencyInjection(builder);

var app = builder.Build();

// add Identity endpoints
app.MapIdentityApi<User>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseExceptionHandler();

// Register Routing endpoints
Routing.MapRoutes(app);

app.Run();


