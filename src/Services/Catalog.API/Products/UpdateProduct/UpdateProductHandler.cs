using FluentValidation;

namespace Catalog.API.Products.UpdateProduct;

public record UpdateProductCommand(
    Guid Id,
    string Name,
    string Description,
    string ImageFile,
    decimal Price,
    string[] Category
) : ICommand<UpdateProductResult>;

public record UpdateProductResult(bool IsSuccess);

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(1000);
        RuleFor(x => x.ImageFile).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Price).GreaterThan(0);
        RuleFor(x => x.Category).NotEmpty();
    }
}

public class UpdateProductCommandHandler (IDocumentSession documentSession, ILogger<UpdateProductCommandHandler> logger): ICommandHandler<UpdateProductCommand, UpdateProductResult>
{

    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating product with ID {ProductId}", command.Id);
    
        var product = await documentSession.LoadAsync<Product>(command.Id, cancellationToken);
        if (product == null)
        {
            logger.LogWarning("Product with ID {ProductId} not found", command.Id);
            throw new ProductNotFoundException(command.Id);
        }

        product.Name = command.Name;
        product.Description = command.Description;
        product.ImageFile = command.ImageFile;
        product.Price = command.Price;
        product.Category.AddRange(command.Category);

        documentSession.Update(product);

        await documentSession.SaveChangesAsync(cancellationToken);

        return await Task.FromResult(new UpdateProductResult(true));
    }
}

