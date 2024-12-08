using AutoMapper;
using LearningStarter.Entities;

namespace Test;

public class TestMapperProfile: Profile
{
    public TestMapperProfile()
    {
        CreateMap<Product, ProductGetDto>().ReverseMap();
        CreateMap<Product, ProductCreateDto>().ReverseMap();
        CreateMap<Product, ProductUpdateDto>().ReverseMap();
    }
}