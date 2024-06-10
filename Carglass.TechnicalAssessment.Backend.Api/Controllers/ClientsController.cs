using Carglass.TechnicalAssessment.Backend.BL;
using Carglass.TechnicalAssessment.Backend.Dtos;
using Carglass.TechnicalAssessment.Backend.Entities;
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
        var clients = await _clientAppService.GetAll();

        return Ok(clients);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var client = await _clientAppService.GetById(id);

        if (client is null)
        {
            return NotFound($"El cliente con el id: {id} no ha sido encontrado");
        }

        return Ok(client);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ClientDto clientDto)
    {
        await _clientAppService.Create(clientDto);

        return CreatedAtAction(nameof(Create), clientDto);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] ClientDto clientDto)
    {
        var client = await _clientAppService.GetById(clientDto.Id);

        if (client is null)
        {
            return BadRequest($"El cliente con el  id: {clientDto.Id} no existe");
        }

        await _clientAppService.Update(clientDto);

        return NoContent();
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> DeleteById(int id)
    {
        var client = await _clientAppService.GetById(id);

        if (client is null)
        {
            return BadRequest($"El cliente con el  id: {id} no existe");
        }

        await _clientAppService.DeleteById(id);

        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] ClientDto clientDto)
    {
        var client = await _clientAppService.GetById(clientDto.Id);

        if (client is null)
        {
            return BadRequest("El cliente que desea borrar no existe");
        }

        await _clientAppService.Delete(clientDto);

        return NoContent();
    }
}