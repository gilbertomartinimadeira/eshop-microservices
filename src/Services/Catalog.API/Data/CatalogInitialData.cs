using Marten.Schema;

namespace Catalog.API.Data;

public class CatalogInitialData : IInitialData

{
    public async Task Populate(IDocumentStore store, CancellationToken cancellation)
    {
        using var session = store.LightweightSession();        
        
        var databaseHasProducts = await session.Query<Product>().AnyAsync();
    
        if (databaseHasProducts)
        {
            return;
        }        

        session.Store(GetProductsToUpsert());

        await session.SaveChangesAsync(cancellation);
    }

    private static IEnumerable<Product> GetProductsToUpsert()
    {
        return new List<Product>
        {
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "Sample Product 1",
                Description = "Description for Sample Product 1",
                ImageFile = "sample1.jpg",
                Price = 9.99m,
                Category = new List<string> { "Category1" }
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "Sample Product 2",
                Description = "Description for Sample Product 2",
                ImageFile = "sample2.jpg",
                Price = 19.99m,
                Category = new List<string> { "Category2" }
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "Sample Product 3",
                Description = "Description for Sample Product 3",
                ImageFile = "sample3.jpg",
                Price = 29.99m,
                Category = new List<string> { "Category3" }
            },new Product
            {
                Id = Guid.NewGuid(),
                Name = "Sample Product 4",
                Description = "Description for Sample Product 4",
                ImageFile = "sample4.jpg",
                Price = 39.99m,
                Category = new List<string> { "Category4" }
            }
        };
    }
}