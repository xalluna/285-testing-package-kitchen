using Bogus;
using LearningStarter.Entities;

namespace Test;

public class ProductFaker
{
    private readonly Faker<Product> ProductConfig = new Faker<Product>()
        .RuleFor(x => x.Name, f => f.Vehicle.Model())
        .RuleFor(x => x.Description, f => f.Vehicle.Type())
        .RuleFor(x => x.Price, f => f.Random.Int(15, 75) * 1_000__00);

    public Product CreateProduct()
    {
        return ProductConfig.Generate();
    }
    
    public IEnumerable<Product> CreateProducts(int quantity)
    {
        for (var i = 0; i < quantity; i++)
        {
            yield return ProductConfig.Generate();
        }
    }

    public void UseSeed(int seed)
    {
        ProductConfig.UseSeed(seed);
    }
}