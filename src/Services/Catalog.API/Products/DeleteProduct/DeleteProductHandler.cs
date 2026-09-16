namespace Catalog.API.Products.DeleteProduct;

public record DeleteProductCommand(Guid Id) : ICommand<DeleteProductResult>;

public record DeleteProductResult(bool IsSuccess);

public class DeleteProductHandler(IDocumentSession documentSession, ILogger<DeleteProductHandler> logger) : ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
    public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling DeleteProductCommand for Id: {Id}", command.Id);
        var product = await documentSession.LoadAsync<Product>(command.Id, cancellationToken);
        if (product is null)
        {
            return await Task.FromResult(new DeleteProductResult(false));
        }

        documentSession.Delete(product);
        await documentSession.SaveChangesAsync(cancellationToken);

        return await Task.FromResult(new DeleteProductResult(true));
    }
}