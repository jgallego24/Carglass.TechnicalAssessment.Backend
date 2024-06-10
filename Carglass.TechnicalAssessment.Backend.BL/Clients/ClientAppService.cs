using AutoMapper;
using Carglass.TechnicalAssessment.Backend.DL.Repositories;
using Carglass.TechnicalAssessment.Backend.Dtos;
using Carglass.TechnicalAssessment.Backend.Entities;
using FluentValidation;

namespace Carglass.TechnicalAssessment.Backend.BL;

public class ClientAppService : IClientAppService
{
    private readonly IClientRepository _clientRepository;
    private readonly IMapper _clientMapper;
    private readonly IValidator<ClientDto> _clientDtoValidator;

    public ClientAppService(IClientRepository clientRepository, IMapper clientMapper, IValidator<ClientDto> clientDtoValidator)
    {
        _clientRepository = clientRepository;
        _clientMapper = clientMapper;
        _clientDtoValidator = clientDtoValidator;
    }

    public async Task<IEnumerable<ClientDto>> GetAll()
    {
        var clients = await _clientRepository.GetAll();
        return _clientMapper.Map<IEnumerable<ClientDto>>(clients);
    }

    public async Task<ClientDto> GetById(params object[] keyValues)
    {
        var client = await _clientRepository.GetById(keyValues);
        return _clientMapper.Map<ClientDto>(client);
    }

    public async Task Create(ClientDto clientDto)
    {
        var clientFromDb = await _clientRepository.GetById(clientDto.Id);

        if (clientFromDb is not null)
        {
            throw new Exception("Ya existe un cliente con este Id");
        }

        if (await _clientRepository.DocNumExists(clientDto.DocNum))
        {
            throw new Exception("El número de documento tiene que ser único");
        }

        ValidateDto(clientDto);

        var newClient = _clientMapper.Map<Client>(clientDto);
        await _clientRepository.Create(newClient);
    }

    public async Task Update(ClientDto clientDto)
    {
        var clientFromDb = await _clientRepository.GetById(clientDto.Id);

        if (clientFromDb is null)
        {
            throw new Exception("No existe ningún cliente con este Id");
        }

        ValidateDto(clientDto);

        var clientEntity = _clientMapper.Map<Client>(clientDto);
        await _clientRepository.Update(clientEntity);
    }

    public async Task DeleteById(int clientId)
    {
        var clientToDelete = await _clientRepository.GetById(clientId) ?? throw new Exception("No existe ningún cliente con este Id");

        await _clientRepository.Delete(clientToDelete);
    }

    public async Task Delete(ClientDto clientDto)
    {
        var clientFromDb = await _clientRepository.GetById(clientDto.Id);

        if (clientFromDb is null)
        {
            throw new Exception("No existe ningún cliente con este Id");
        }

        ValidateDto(clientDto);

        var clientToDelete = _clientMapper.Map<Client>(clientDto);

        await _clientRepository.Delete(clientToDelete);
    }

    private void ValidateDto(ClientDto clientDto)
    {
        var validationResult = _clientDtoValidator.Validate(clientDto);
        if (validationResult.Errors.Any())
        {
            string toShowErrors = string.Join("; ", validationResult.Errors.Select(s => s.ErrorMessage));
            throw new Exception($"El cliente especificado no cumple los requisitos de validación. Errores: '{toShowErrors}'");
        }
    }
}