using cqrs_mediatr.Routing;
using cqrs_mediatr.Configuration;
using cqrs_mediatr.Domain;

var builder = WebApplication.CreateBuilder(args);

// Registering MediatR add pipeline behaviour
builder.Services.AddMediator(options =>
{
    options.ServiceLifetime = ServiceLifetime.Scoped;
});

// Registeering DI
builder.Services.AddDependencyInjection();

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


