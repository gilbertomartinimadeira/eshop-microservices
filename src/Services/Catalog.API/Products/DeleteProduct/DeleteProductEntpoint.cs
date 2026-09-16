namespace Catalog.API.Products.DeleteProduct;

//public record DeleteProductRequest(Guid Id);

public record DeleteProductResponse(bool IsSuccess);

public class DeleteProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/products/{id}", 
        async (Guid id, ISender sender) =>
        {            
            var result = await sender.Send(new DeleteProductCommand(id), CancellationToken.None);

            if (!result.IsSuccess)
            {
                return Results.NotFound();
            }

            var response = result.Adapt<DeleteProductResponse>();

            return Results.NoContent();
        }).WithName("DeleteProduct")
        .WithSummary("Deletes an existing product")
        .Produces<DeleteProductResponse>(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}