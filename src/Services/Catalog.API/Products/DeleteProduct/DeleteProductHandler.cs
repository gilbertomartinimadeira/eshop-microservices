namespace Catalog.API.Products.DeleteProduct;

public record DeleteProductCommand(Guid Id) : ICommand<DeleteProductResult>;

public record DeleteProductResult(bool IsSuccess);

public class DeleteProductHandler(IDocumentSession documentSession) : ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
    public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
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