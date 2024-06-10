using Carglass.TechnicalAssessment.Backend.BL.Products;
using Carglass.TechnicalAssessment.Backend.Dtos;
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
        return Ok(await _productAppService.GetAll());
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        return Ok(await _productAppService.GetById(id));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductDto productDto)
    {
        await _productAppService.Create(productDto);

        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] ProductDto productDto)
    {
        await _productAppService.Update(productDto);

        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] ProductDto productDto)
    {
        await _productAppService.Delete(productDto);

        return Ok();
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> DeleteById(int id)
    {
        await _productAppService.DeleteById(id);

        return Ok();
    }
}