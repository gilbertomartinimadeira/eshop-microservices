namespace Catalog.API.Products.GetProductsByCategory;


public record GetProductsByCategoryQuery(string Category) : IQuery<GetProductsByCategoryResult>;

public record GetProductsByCategoryResult(IEnumerable<Product> Products);

internal class GetProductsByCategoryQueryHandler : IQueryHandler<GetProductsByCategoryQuery, GetProductsByCategoryResult>
{
    private readonly IDocumentSession _documentSession;
    private readonly ILogger<GetProductsByCategoryQueryHandler> _logger;

    public GetProductsByCategoryQueryHandler(IDocumentSession documentSession,
                                             ILogger<GetProductsByCategoryQueryHandler> logger)
    {
        _documentSession = documentSession;
        _logger = logger;
    }

    public async Task<GetProductsByCategoryResult> Handle(GetProductsByCategoryQuery query, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetProductsByCategoryQuery for category: {Category}", query.Category);

        var products = await _documentSession.Query<Product>()
                                       .Where(p => p.Category.Contains(query.Category))
                                       .ToListAsync(cancellationToken);

        return new GetProductsByCategoryResult(products);
    }
}