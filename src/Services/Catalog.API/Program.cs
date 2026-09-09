using Carter;
using Catalog.API.Products.CreateProduct;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddMediatR(configuration =>
    configuration.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.AddCarter(configurator: configurator =>
{
    configurator.WithModule<CreateProductEndpoint>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapCarter();



//app.UseHttpsRedirection();

app.Run();
