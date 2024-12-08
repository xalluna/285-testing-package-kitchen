using System;
using System.Collections.Generic;
using System.Linq;
using LearningStarter.Common;
using LearningStarter.Common.EntityController;
using LearningStarter.Entities;
using LearningStarter.Services;
using Microsoft.AspNetCore.Mvc;

namespace LearningStarter.Controllers;


[ApiController]
[Route("api/foos")]
public class FoosController(IProductsService productsService) : EntityController<Product>
{
    [HttpGet]
    public ActionResult<Response<List<ProductGetDto>>> GetAll()
    {
        var response = productsService.GetAll();
        return Ok(response);
    }

    [HttpGet("{id}")]
    public ActionResult<Response<ProductGetDto>> GetById([FromRoute] int id)
    {
        var response = productsService.GetById(id);
        return Ok(response);
    }

    [HttpPost]
    public ActionResult<Response<ProductGetDto>> Create([FromBody] ProductCreateDto productCreateDto)
    {
        var response = productsService.Create(productCreateDto);
        return Created("", response);
    }

    [HttpPut("{id}")]
    public ActionResult<Response<ProductGetDto>> Update(
        [FromRoute] int id, 
        [FromBody] ProductUpdateDto productUpdateDto)
    {
        var response = productsService.Update(id, productUpdateDto);
        return Ok(response);
    }

    [HttpDelete("{id}")]
    public ActionResult<Response> DeleteById([FromRoute] int id)
    {
        var response = productsService.Delete(id);
        return Ok(response);
    }
}
