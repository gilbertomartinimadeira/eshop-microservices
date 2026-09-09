using BuildingBlocks.CQRS;
using Catalog.API.Models;

namespace Catalog.API.Products.CreateProduct;

public record CreateProductCommand(
    string Name,
    List<string> Category,
    string Description,
    string ImageFile,
    decimal Price
) : ICommand<CreateProductResult>;

public record CreateProductResult (Guid Id);

internal class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    private readonly ILogger<CreateProductCommandHandler> _logger;
    public CreateProductCommandHandler(ILogger<CreateProductCommandHandler> logger)
    {
        _logger = logger;
    }

    public Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        // create a product from the command
        _logger.LogInformation("Creating a new product with name: {Name}", command.Name);
        var product = new Product
        {       
            Id = Guid.NewGuid(), //simulating database-generated Id
            Name = command.Name,
            Category = command.Category,
            Description = command.Description,
            ImageFile = command.ImageFile,
            Price = command.Price
        };

        //TODO: Implement saving to database

        return Task.FromResult(new CreateProductResult(product.Id));
        
    }
}