using MediatR;

namespace Catalog.API.Products.CreateProduct;

public record CreateProductCommand(
    string Name,
    List<string> Category,
    string Description,
    string ImageFile,
    decimal Price
) : IRequest<CreateProductResult>;

public record CreateProductResult (Guid Id);

internal class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, CreateProductResult>
{
    private readonly ILogger<CreateProductCommandHandler> _logger;
    public CreateProductCommandHandler(ILogger<CreateProductCommandHandler> logger)
    {
        _logger = logger;
    }

    public Task<CreateProductResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        Console.WriteLine("Handling CreateProductCommand");
        throw new NotImplementedException();
    }
}