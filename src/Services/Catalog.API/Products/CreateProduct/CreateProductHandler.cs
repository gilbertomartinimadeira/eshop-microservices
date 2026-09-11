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
    private readonly IDocumentSession _session;
    public CreateProductCommandHandler(
        ILogger<CreateProductCommandHandler> logger,
        IDocumentSession session)
    {
        _logger = logger;
        _session = session;
    }

    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        // create a product from the command
        _logger.LogInformation("Creating a new product with name: {Name}", command.Name);
        var product = new Product
        {               
            Name = command.Name,
            Category = command.Category,
            Description = command.Description,
            ImageFile = command.ImageFile,
            Price = command.Price
        };

        // Implement saving to database
        _session.Store(product);
        await _session.SaveChangesAsync(cancellationToken);

        return new CreateProductResult(product.Id);
        
    }
}