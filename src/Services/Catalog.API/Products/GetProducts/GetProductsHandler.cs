using Marten.Pagination;

namespace Catalog.API.Products.GetProducts;

public record GetProductsQuery(int pageNumber = 1, int pageSize = 2) : IQuery<GetProductsResult>;

public record GetProductsResult(IEnumerable<Product> Products);

public class GetProductsQueryHandler (
        IDocumentSession _documentSession ) : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    => new GetProductsResult(await _documentSession.Query<Product>().ToPagedListAsync(query.pageNumber, query.pageSize, cancellationToken));
}
