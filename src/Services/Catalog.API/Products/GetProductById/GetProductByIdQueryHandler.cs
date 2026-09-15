namespace Catalog.API.Products.GetProductById;

public record GetProductByIdQuery(Guid Id) : IQuery<GetProductByIdResult>;

public record GetProductByIdResult(Product product);

public class GetProductByIdQueryHandler(IDocumentSession documentSession, ILogger<GetProductByIdQueryHandler> logger ) 
           : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
{
    public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling GetProductByIdQuery for Id: {Id}", query.Id);

        var product = await documentSession.LoadAsync<Product>(query.Id, cancellationToken);

        if(product == null)
        {
            logger.LogWarning("Product with Id: {Id} not found", query.Id);
            throw new ProductNotFoundException();
        }

        return new GetProductByIdResult(product!);  
    }
}