using DewaEShop.Application.Configuration;
using DewaEShop.Configuration;
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
//app.MapIdentityApi<User>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "DewaEShop API v1.0");
        c.SwaggerEndpoint("/swagger/v2/swagger.json", "DewaEShop API v2.0");
        c.SwaggerEndpoint("/swagger/v3/swagger.json", "DewaEShop API v3.0");
        c.SwaggerEndpoint("/swagger/v4/swagger.json", "DewaEShop API v4.0");
        c.SwaggerEndpoint("/swagger/v5/swagger.json", "DewaEShop API v5.0");
    });
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseExceptionHandler();

// Register Routing endpoints
app.UseEndpoints(endpoints =>
{
    endpoints.MapProductApi();
    endpoints.MapCartApi();
    endpoints.MapUsersApi();
    endpoints.MapSendGridApi();
});

app.Run();


