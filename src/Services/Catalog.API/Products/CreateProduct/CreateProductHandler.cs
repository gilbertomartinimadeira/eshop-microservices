using FluentValidation;

namespace Catalog.API.Products.CreateProduct;

public record CreateProductCommand(
    string Name,
    List<string> Category,
    string Description,
    string ImageFile,
    decimal Price
) : ICommand<CreateProductResult>;

public record CreateProductResult (Guid Id);

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Product name is required.");
        RuleFor(x => x.Category).NotEmpty().WithMessage("Product category is required.");
        RuleFor(x => x.Description).NotEmpty().WithMessage("Product description is required.");
        RuleFor(x => x.ImageFile).NotEmpty().WithMessage("Product image file is required.");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Product price must be greater than zero.");
    }
}

internal class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    private readonly ILogger<CreateProductCommandHandler> _logger;
    private readonly IDocumentSession _session;
    private readonly IValidator<CreateProductCommand> _validator;
    public CreateProductCommandHandler(
        ILogger<CreateProductCommandHandler> logger,
        IDocumentSession session, IValidator<CreateProductCommand> validator)
    {
        _logger = logger;
        _session = session;
        _validator = validator;
    }

    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        // validate the command
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

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