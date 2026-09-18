using Catalog.API.Products.GetProducts;
using FluentValidation;
using BuildingBlocks.Behaviors;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.AspNetCore.Diagnostics;
using BuildingBlocks.Exceptions.Handlers;
using Microsoft.Extensions.Options;
using Catalog.API.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddMediatR(configuration => {
    configuration.RegisterServicesFromAssembly(typeof(Program).Assembly);
    configuration.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    configuration.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
});

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);


builder.Services.AddCarter(
     new DependencyContextAssemblyCatalog(
        typeof(Program).Assembly, typeof(GetProductsEndpoint).Assembly
     )
);

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

builder.Services.AddMarten(options =>
{
    options.Connection(builder.Configuration.GetConnectionString("Database")!);    
}).UseLightweightSessions();

if (builder.Environment.IsDevelopment())
{
    builder.Services.InitializeMartenWith<CatalogInitialData>();
}


builder.Services.AddExceptionHandler<CustomExceptionHandler>();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{

    app.MapOpenApi();
}


app.UseHsts();

app.MapCarter();

app.UseExceptionHandler( _ => {});

//app.UseHttpsRedirection();

app.Run();
