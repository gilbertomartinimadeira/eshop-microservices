namespace Catalog.API.Products.UpdateProduct;

public record UpdateProductRequest(
    Guid Id,
    string Name,
    string Description,
    string ImageFile,
    decimal Price,
    string[] Category
);

public record UpdateProductResponse(bool IsSuccess);

public class UpdateProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/products", 
        async (UpdateProductRequest request, ISender sender) =>
        {
            var command = request.Adapt<UpdateProductCommand>();

            var result = await sender.Send(command, CancellationToken.None);

            if (!result.IsSuccess)
            {
                return Results.NotFound();
            }
            var response = result.Adapt<UpdateProductResponse>();

            return Results.NoContent();
        }).WithName("UpdateProduct")
        .WithSummary("Updates an existing product")
        .Produces<UpdateProductResponse>(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}