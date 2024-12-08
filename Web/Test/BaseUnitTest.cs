using AutoMapper;
using LearningStarter.Data;
using LearningStarter.Services;
using Microsoft.EntityFrameworkCore;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace Test;

public class BaseUnitTest
{
    protected Container Container { get; set; }
    protected Scope Scope { get; set; }
    protected DataContext DataContext => Container.GetInstance<DataContext>();
    protected IProductsService ProductsService => Container.GetInstance<IProductsService>();
    protected IMapper Mapper => Container.GetInstance<IMapper>();

    [SetUp]
    public virtual void Setup()
    {
        var container = new Container();

        container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();
        
        container.Register(() =>
        {
            var options = new DbContextOptionsBuilder<DataContext>();
            options.UseInMemoryDatabase(Guid.NewGuid().ToString());
            return new DataContext(options.Options);
        }, Lifestyle.Scoped);
        
        container.Register<IProductsService, Answer>(Lifestyle.Scoped);
        container.Register(() =>
        {
            var mapperConfiguration = new MapperConfiguration(config => config.AddProfile<TestMapperProfile>());
            return mapperConfiguration.CreateMapper();
        });
        
        Container = container;
        Scope = AsyncScopedLifestyle.BeginScope(container);
    }

    [TearDown]
    public void TearDown()
    {
        Scope.Dispose();
    }
}