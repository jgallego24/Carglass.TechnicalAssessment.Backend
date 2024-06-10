using Carglass.TechnicalAssessment.Backend.BL;
using Carglass.TechnicalAssessment.Backend.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Carglass.TechnicalAssessment.Backend.Api.Controllers;

[ApiController]
[Route("clients")]
public class ClientsController : ControllerBase
{
    private readonly IClientAppService _clientAppService;

    public ClientsController(IClientAppService clientAppService)
    {
        _clientAppService = clientAppService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _clientAppService.GetAll());
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        return Ok(await _clientAppService.GetById(id));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ClientDto clientDto)
    {
        await _clientAppService.Create(clientDto);

        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] ClientDto clientDto)
    {
        await _clientAppService.Update(clientDto);

        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] ClientDto clientDto)
    {
        await _clientAppService.Delete(clientDto);

        return Ok();
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> DeleteById(int id)
    {
        await _clientAppService.DeleteById(id);

        return Ok();
    }
}