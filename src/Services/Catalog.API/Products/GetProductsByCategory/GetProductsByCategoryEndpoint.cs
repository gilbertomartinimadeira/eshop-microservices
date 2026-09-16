namespace Catalog.API.Products.GetProductsByCategory;

public record GetProductsByCategoryResponse(IEnumerable<Product> Products);

public class GetProductsByCategoryEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/by-category/{category}", 
        async (string category, ISender sender) =>
        {
            var result = await sender.Send(new GetProductsByCategoryQuery(category), CancellationToken.None);

            var response = result.Adapt<GetProductsByCategoryResponse>();
            return Results.Ok(response);
            
        }).WithName("GetProductsByCategory")
          .WithDescription("Gets products by category.")
          .Produces<GetProductsByCategoryResponse>(StatusCodes.Status200OK)
          .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}