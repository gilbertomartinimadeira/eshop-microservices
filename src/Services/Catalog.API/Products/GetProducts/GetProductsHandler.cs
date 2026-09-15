namespace Catalog.API.Products.GetProducts;

public record GetProductsQuery : IQuery<GetProductsResult>;

public record GetProductsResult(IEnumerable<Product> Products);

public class GetProductsQueryHandler (
        IDocumentSession _documentSession,
        ILogger<GetProductsQueryHandler> _logger) : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"GetProductsQueryHandler.Handle called with query {query}", cancellationToken);

        return new GetProductsResult(await _documentSession.Query<Product>().ToListAsync(cancellationToken));

        
    }
}
