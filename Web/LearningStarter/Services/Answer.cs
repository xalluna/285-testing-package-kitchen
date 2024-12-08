using System.Collections.Generic;
using System.Linq;
using LearningStarter.Common;
using LearningStarter.Data;
using LearningStarter.Entities;
using Microsoft.AspNetCore.Mvc;

namespace LearningStarter.Services;

public class Answer(DataContext dataContext) : IProductsService
{
    public Response<List<ProductGetDto>> GetAll()
    {
        var response = new Response<List<ProductGetDto>>();

        response.Data = dataContext
            .Set<Product>()
            .Select(x => new ProductGetDto
            {
                Id = x.Id,
                Name = x.Name,
                Description  = x.Description,
                Price = x.Price
            })
            .ToList();

        return response;
    }
    
    public Response<ProductGetDto> GetById(int id)
    {
        var response = new Response<ProductGetDto>();

        var product = dataContext.Set<Product>().FirstOrDefault(x => x.Id == id);

        if (product == null)
        {
            response.AddError("id", "There was a problem finding the product.");
            return response;
        }

        var productGetDto = new ProductGetDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price
        };

        response.Data = productGetDto;

        return response;
    }
    
    public Response<ProductGetDto> Create(ProductCreateDto productCreateDto)
    {
        var response = new Response<ProductGetDto>();

        if (string.IsNullOrEmpty(productCreateDto.Name))
        {
            response.AddError("name", "Name cannot be empty.");
        }
        
        if (response.HasErrors)
        {
            return response;
        }

        var productToCreate = new Product
        {
            Name = productCreateDto.Name,
            Description = productCreateDto.Description,
            Price = productCreateDto.Price,
        };
        
        dataContext.Set<Product>().Add(productToCreate);
        dataContext.SaveChanges();

        var productGetDto = new ProductGetDto
        {
            Id = productToCreate.Id,
            Name = productToCreate.Name,
            Description = productCreateDto.Description,
            Price = productCreateDto.Price
        };

        response.Data = productGetDto;

        return response;
    }
    
    public Response<ProductGetDto> Update(int id, ProductUpdateDto productUpdateDto)
    {
        var response = new Response<ProductGetDto>();

        if (productUpdateDto == null)
        {
            response.AddError("id", "There was a problem editing the product.");
            return response;
        }
        
        var productToEdit = dataContext.Set<Product>().FirstOrDefault(x => x.Id == id);

        if (productToEdit == null)
        {
            response.AddError("id", "Could not find product to edit.");
            return response;
        }

        if (string.IsNullOrEmpty(productUpdateDto.Name))
        {
            response.AddError("name", "Name cannot be empty.");
        }

        if (response.HasErrors)
        {
            return response;
        }

        productToEdit.Name = productUpdateDto.Name;
        productToEdit.Description = productUpdateDto.Description;
        productToEdit.Price = productUpdateDto.Price;

        dataContext.SaveChanges();

        var productGetDto = new ProductGetDto
        {
            Id = productToEdit.Id,
            Name = productToEdit.Name,
            Description = productToEdit.Description,
            Price = productToEdit.Price
        };

        response.Data = productGetDto;
        return response;
    }
    
    public Response Delete(int id)
    {
        var response = new Response();

        var product = dataContext.Set<Product>().FirstOrDefault(x => x.Id == id);

        if (product == null)
        {
            response.AddError("id", "There was a problem deleting the product.");
            return response;
        }

        dataContext.Set<Product>().Remove(product);
        dataContext.SaveChanges();

        return response;
    }
}