using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LearningStarter.Common;
using LearningStarter.Common.EntityController;
using LearningStarter.Entities;
using LearningStarter.Services;
using Microsoft.AspNetCore.Mvc;

namespace LearningStarter.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController(IProductsService productsService) : ControllerBase
{
    [HttpGet]
    public ActionResult<Response<List<ProductGetDto>>> GetAll()
    {
        var response = productsService.GetAll();
        return Ok(response);
    }

    [HttpGet("{id}")]
    public IActionResult GetById([FromRoute] int id)
    {
        var response = productsService.GetById(id);
        return Ok(response);
    }

    [HttpPost]
    public IActionResult Create([FromBody] ProductCreateDto productCreateDto)
    {
        var response = productsService.Create(productCreateDto);
        return Created("", response);
    }

    [HttpPut("{id}")]
    public IActionResult Edit(
        [FromRoute] int id, 
        [FromBody] ProductUpdateDto productUpdateDto)
    {
        var response = productsService.Update(id, productUpdateDto);
        return Ok(response);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete([FromRoute] int id)
    {
        var response = productsService.Delete(id);
        return Ok(response);
    }

    [HttpGet("foo")]
    public IActionResult GetFoo()
    {
        var type = typeof(ControllerMethods)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(x => x.FieldType == typeof(string))
            .Select(x => (string) x.GetValue(null));
        
        return Ok(type);
    }

    [HttpGet("foo2")]
    public ActionResult<Response<bool>> GetFoo2()
    {
        return Ok(ControllerMethodsExtensions.ValidateControllerConfiguration());
    }
}
