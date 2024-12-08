using LearningStarter.Entities;

namespace Test;

public class ProductUnitTest : BaseUnitTest
{
    private const int Seed1 = 123456789;
    private const int Seed2 = 987654321;
    private const int Seed3 = 43219876;
    private const int Seed4 = 12349876;

    private ProductFaker _productFaker { get; set; }

    [SetUp]
    public override void Setup()
    {
        base.Setup();
        _productFaker = new ProductFaker();
    }

    [Test]
    [TestCase(1, Seed1)]
    [TestCase(5, Seed2)]
    [TestCase(10, Seed3)]
    [TestCase(25, Seed4)]
    public void GetAll(int seededAmount, int seed)
    {
        SeedProducts(seededAmount, seed);
        
        var result = ProductsService.GetAll();
        
        Assert.Multiple(() =>
        {
            Assert.That(result.Data, Has.Count.EqualTo(seededAmount));
            Assert.That(result.HasErrors, Is.False);
        });

        var products = DataContext.Set<Product>().ToList();
        var productTuples = products.Select((x, i) => (x, result.Data.ElementAt(i)));
        
        foreach (var (productFromDatabase, productFromResult) in productTuples)
        {
            Assert.Multiple(() =>
            {
                Assert.That(productFromResult.Name, Is.EqualTo(productFromDatabase.Name));
                Assert.That(productFromResult.Description, Is.EqualTo(productFromDatabase.Description));
                Assert.That(productFromResult.Price, Is.EqualTo(productFromDatabase.Price));
            });
        }
    }

    [Test]
    [TestCase(1, Seed1)]
    [TestCase(5, Seed2)]
    [TestCase(10, Seed3)]
    [TestCase(25, Seed4)]
    public void GetById(int seededAmount, int seed)
    {
        SeedProducts(seededAmount, seed);
        
        var id = GetRandomId(seededAmount, seed);
        
        var result = ProductsService.GetById(id);

        if (id > seededAmount)
        {
            Assert.That(result.HasErrors, Is.True);
            return;
        }
        
        Assert.That(result.HasErrors, Is.False);
        
        var productFromDatabase = DataContext.Set<Product>().First(x => x.Id == id);
        
        
        Assert.Multiple(() =>
        {
            Assert.That(result.Data.Name, Is.EqualTo(productFromDatabase.Name));
            Assert.That(result.Data.Description, Is.EqualTo(productFromDatabase.Description));
            Assert.That(result.Data.Price, Is.EqualTo(productFromDatabase.Price));
        });
    }

    [Test]
    [TestCase(Seed1)]
    [TestCase(Seed2)]
    [TestCase(Seed3, true)]
    [TestCase(Seed4, true)]
    public void Create(int seed, bool withEmptyName = false)
    {
        var product = _productFaker.CreateProduct();

        if (withEmptyName)
        {
            product.Name = null;
        }
        
        var createDto = Mapper.Map<ProductCreateDto>(product);
        
        var result = ProductsService.Create(createDto);

        if (withEmptyName)
        {
            Assert.That(result.HasErrors, Is.True);
            return;
        }
        
        Assert.That(result.HasErrors, Is.False);
        
        var productFromDatabase = DataContext.Set<Product>().First();
        
        Assert.Multiple(() =>
        {
            Assert.That(productFromDatabase.Name, Is.EqualTo(product.Name));
            Assert.That(productFromDatabase.Description, Is.EqualTo(product.Description));
            Assert.That(productFromDatabase.Price, Is.EqualTo(product.Price));
            
            Assert.That(productFromDatabase.Name, Is.EqualTo(result.Data.Name));
            Assert.That(productFromDatabase.Description, Is.EqualTo(result.Data.Description));
            Assert.That(productFromDatabase.Price, Is.EqualTo(result.Data.Price));
        });
    }
    
    [Test]
    [TestCase(1, Seed1)]
    [TestCase(5, Seed2)]
    [TestCase(10, Seed3, true)]
    [TestCase(25, Seed4, true)]
    public void Update(int seededAmount,int seed, bool withEmptyName = false)
    {
        SeedProducts(seededAmount, seed);
        
        var id = GetRandomId(seededAmount, seed);
        
        var product = _productFaker.CreateProduct();

        if (withEmptyName)
        {
            product.Name = null;
        }
        
        var updateDto = Mapper.Map<ProductUpdateDto>(product);
        
        var result = ProductsService.Update(id, updateDto);

        if (withEmptyName || id > seededAmount)
        {
            Assert.That(result.HasErrors, Is.True);
            return;
        }
        
        var productFromDatabase = DataContext.Set<Product>().First(x => x.Id == id);
        
        Assert.That(result.HasErrors, Is.False);
        
        Assert.Multiple(() =>
        {
            Assert.That(productFromDatabase.Name, Is.EqualTo(product.Name));
            Assert.That(productFromDatabase.Description, Is.EqualTo(product.Description));
            Assert.That(productFromDatabase.Price, Is.EqualTo(product.Price));
            
            Assert.That(productFromDatabase.Name, Is.EqualTo(result.Data.Name));
            Assert.That(productFromDatabase.Description, Is.EqualTo(result.Data.Description));
            Assert.That(productFromDatabase.Price, Is.EqualTo(result.Data.Price));
        });
    }

    [Test]
    [TestCase(1, Seed1)]
    [TestCase(5, Seed2)]
    [TestCase(10, Seed3)]
    [TestCase(25, Seed4)]
    public void DeleteById(int seededAmount, int seed)
    {
        SeedProducts(seededAmount, seed);
        
        var id = GetRandomId(seededAmount, seed);
        
        var result = ProductsService.Delete(id);

        if (id > seededAmount)
        {
            Assert.That(result.HasErrors, Is.True);
            return;
        }
        
        Assert.That(result.HasErrors, Is.False);
        
        var productFromDatabase = DataContext.Set<Product>().FirstOrDefault(x => x.Id == id);
        
        Assert.That(productFromDatabase, Is.Null);
    }
    
    private void SeedProducts(int quantity, int seed)
    {
        var dataContext = DataContext;
        
        _productFaker.UseSeed(seed);
        var seededProducts = _productFaker.CreateProducts(quantity);

        dataContext.Set<Product>().AddRange(seededProducts);
        dataContext.SaveChanges();
    }

    private static int GetRandomId(int amount, int seed)
    {
        var random = new Random(seed);
        return random.Next(1, (int)Math.Floor(amount * 1.25));
    }
}