using Carglass.TechnicalAssessment.Backend.BL.Products;
using Carglass.TechnicalAssessment.Backend.Dtos;
using Carglass.TechnicalAssessment.Backend.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Carglass.TechnicalAssessment.Backend.Api.Controllers;

[ApiController]
[Route("products")]
public class ProductsController : ControllerBase
{
    private readonly IProductAppService _productAppService;

    public ProductsController(IProductAppService productAppService)
    {
        _productAppService = productAppService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _productAppService.GetAll();

        return Ok(products);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _productAppService.GetById(id);

        if (product is null)
        {
            return NotFound($"El producto con el id: {id} no ha sido encontrado");
        }

        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductDto productDto)
    {
        await _productAppService.Create(productDto);

        return CreatedAtAction(nameof(Create), productDto);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] ProductDto productDto)
    {
        var product = await _productAppService.GetById(productDto.Id);

        if (product is null)
        {
            return BadRequest($"El cliente con el  id: {productDto.Id} no existe");
        }

        await _productAppService.Update(productDto);

        return NoContent();
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> DeleteById(int id)
    {
        var product = await _productAppService.GetById(id);

        if (product is null)
        {
            return BadRequest($"El producto con el  id: {id} no existe");
        }

        await _productAppService.DeleteById(id);

        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] ProductDto productDto)
    {
        var product = await _productAppService.GetById(productDto.Id);

        if (product is null)
        {
            return BadRequest("El producto que desea borrar no existe");
        }

        await _productAppService.Delete(productDto);

        return NoContent();
    }
}