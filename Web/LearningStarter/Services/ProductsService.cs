using System.Collections.Generic;
using LearningStarter.Common;
using LearningStarter.Data;
using LearningStarter.Entities;
using Microsoft.AspNetCore.Mvc;

namespace LearningStarter.Services;

public interface IProductsService
{
    Response<List<ProductGetDto>> GetAll();
    Response<ProductGetDto> GetById(int id);
    Response<ProductGetDto> Create(ProductCreateDto productCreateDto);
    Response<ProductGetDto> Update(int id, ProductUpdateDto productUpdateDto);
    Response Delete(int id);
}

public class ProductsService(DataContext dataContext) : IProductsService
{
    public Response<List<ProductGetDto>> GetAll()
    {
        return new Response<List<ProductGetDto>>();
    }
    
    public Response<ProductGetDto> GetById(int id)
    {
        return new Response<ProductGetDto>();
    }
    
    public Response<ProductGetDto> Create(ProductCreateDto productCreateDto)
    {
        return new Response<ProductGetDto>();
    }
    
    public Response<ProductGetDto> Update(int id, ProductUpdateDto productUpdateDto)
    {
        return new Response<ProductGetDto>();
    }
    
    public Response Delete(int id)
    {
        return new Response();
    }
}