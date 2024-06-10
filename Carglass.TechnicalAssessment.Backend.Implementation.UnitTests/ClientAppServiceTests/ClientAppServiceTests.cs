using AutoMapper;
using Carglass.TechnicalAssessment.Backend.BL;
using Carglass.TechnicalAssessment.Backend.DL.Repositories;
using Carglass.TechnicalAssessment.Backend.Dtos;
using Carglass.TechnicalAssessment.Backend.Entities;
using FluentValidation;
using Moq;

namespace Carglass.TechnicalAssessment.Backend.Implementation.UnitTests;

public class ClientAppServiceTests
{
    private readonly Mock<IClientRepository> _mockClientRepository;
    private readonly Mock<IMapper> _clientMapper;
    private readonly Mock<IValidator<ClientDto>> _clientDtoValidator;
    private readonly ClientAppService _clientAppService;

    public ClientAppServiceTests()
    {
        _mockClientRepository = new Mock<IClientRepository>();
        _clientMapper = new Mock<IMapper>();
        _clientDtoValidator = new Mock<IValidator<ClientDto>>();
        _clientAppService = new ClientAppService(_mockClientRepository.Object, _clientMapper.Object, _clientDtoValidator.Object);
    }

    [Fact]
    public async Task GetAll_ShouldReturnAllClients()
    {
        // Arrange
        var clients = new List<Client>
        {
            new Client { Id = 1, DocType = "nif", DocNum = "123456789P", Email = "test1@test.com", GivenName = "Unit", FamilyName1 = "Test", Phone = "123456789"},
            new Client { Id = 2, DocType = "nif", DocNum = "87654321X", Email = "test2@test.com", GivenName = "Test", FamilyName1 = "Unit", Phone = "123456789"}
        };

        var clientsDto = new List<ClientDto>
        {
            new ClientDto { Id = 1, DocType = "nif", DocNum = "123456789P", Email = "test1@test.com", GivenName = "Unit", FamilyName1 = "Test", Phone = "123456789"},
            new ClientDto { Id = 2, DocType = "nif", DocNum = "87654321X", Email = "test2@test.com", GivenName = "Test", FamilyName1 = "Unit", Phone = "123456789"}
        };

        _mockClientRepository.Setup(repository => repository.GetAll()).ReturnsAsync(clients);
        _clientMapper.Setup(mapper => mapper.Map<IEnumerable<ClientDto>>(It.IsAny<IEnumerable<Client>>())).Returns(clientsDto);

        // Act
        var result = await _clientAppService.GetAll();

        // Assert
        Assert.Equal(clientsDto, result);
    }

    [Fact]
    public async Task GetById_ShouldReturnAClient()
    {
        // Arrange
        var client = new Client { Id = 1, DocType = "nif", DocNum = "123456789P", Email = "test1@test.com", GivenName = "Unit", FamilyName1 = "Test", Phone = "123456789" };

        var clientDto = new ClientDto { Id = 1, DocType = "nif", DocNum = "123456789P", Email = "test1@test.com", GivenName = "Unit", FamilyName1 = "Test", Phone = "123456789" };

        _mockClientRepository.Setup(repository => repository.GetById(It.IsAny<object>())).ReturnsAsync(client);
        _clientMapper.Setup(mapper => mapper.Map<ClientDto>(It.IsAny<Client>())).Returns(clientDto);

        // Act
        var result = await _clientAppService.GetById(client.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(clientDto.Id, result.Id);
        Assert.Equal(clientDto, result);
    }

    [Fact]
    public async Task Create_ShouldCreateClient()
    {
        // Arrange
        var client = new Client { Id = 1, DocType = "nif", DocNum = "123456789P", Email = "test1@test.com", GivenName = "Unit", FamilyName1 = "Test", Phone = "123456789" };
        var clientDto = new ClientDto { Id = 1, DocType = "nif", DocNum = "123456789P", Email = "test1@test.com", GivenName = "Unit", FamilyName1 = "Test", Phone = "123456789" };

        _mockClientRepository.Setup(repository => repository.DocNumExists(It.IsAny<string>())).ReturnsAsync(false);
        _clientDtoValidator.Setup(validator => validator.Validate(It.IsAny<ClientDto>())).Returns(new FluentValidation.Results.ValidationResult());
        _clientMapper.Setup(mapper => mapper.Map<Client>(It.IsAny<ClientDto>())).Returns(client);

        // Act
        await _clientAppService.Create(clientDto);

        // Assert
        _mockClientRepository.Verify(repository => repository.GetById(It.IsAny<object>()), Times.Once);
        _mockClientRepository.Verify(repository => repository.DocNumExists(It.IsAny<string>()), Times.Once);
        _mockClientRepository.Verify(repository => repository.Create(client), Times.Once);
        _clientDtoValidator.Verify(validator => validator.Validate(It.IsAny<ClientDto>()), Times.Once);
        _clientMapper.Verify(mapper => mapper.Map<Client>(It.IsAny<ClientDto>()), Times.Once);
    }

    [Fact]
    public async Task Update_ShouldUpdateClient()
    {
        // Arrange
        var client = new Client { Id = 1, DocType = "nif", DocNum = "123456789P", Email = "test1@test.com", GivenName = "Unit", FamilyName1 = "Test", Phone = "123456789" };
        var clientDto = new ClientDto { Id = 1, DocType = "nif", DocNum = "123456789P", Email = "test1@test.com", GivenName = "Unit", FamilyName1 = "Test", Phone = "123456789" };

        _mockClientRepository.Setup(repository => repository.GetById(It.IsAny<object>())).ReturnsAsync(client);
        _clientDtoValidator.Setup(validator => validator.Validate(It.IsAny<ClientDto>())).Returns(new FluentValidation.Results.ValidationResult());
        _clientMapper.Setup(mapper => mapper.Map<Client>(It.IsAny<ClientDto>())).Returns(client);

        // Act
        await _clientAppService.Update(clientDto);

        // Assert
        _mockClientRepository.Verify(repository => repository.GetById(It.IsAny<object>()), Times.Once);
        _mockClientRepository.Verify(repository => repository.Update(client), Times.Once);
        _clientDtoValidator.Verify(validator => validator.Validate(It.IsAny<ClientDto>()), Times.Once);
        _clientMapper.Verify(mapper => mapper.Map<Client>(It.IsAny<ClientDto>()), Times.Once);
    }

    [Fact]
    public async Task DeleteById_ShouldDeleteClientById()
    {
        // Arrange
        var client = new Client { Id = 1, DocType = "nif", DocNum = "123456789P", Email = "test1@test.com", GivenName = "Unit", FamilyName1 = "Test", Phone = "123456789" };
        var clientId = 1;

        _mockClientRepository.Setup(repository => repository.GetById(It.IsAny<object>())).ReturnsAsync(client);

        // Act
        await _clientAppService.DeleteById(clientId);

        // Assert
        _mockClientRepository.Verify(repository => repository.GetById(It.IsAny<object>()), Times.Once);
        _mockClientRepository.Verify(repository => repository.Delete(It.IsAny<Client>()), Times.Once);
    }

    [Fact]
    public async Task Delete_ShouldDeleteClient()
    {
        // Arrange
        var client = new Client { Id = 1, DocType = "nif", DocNum = "123456789P", Email = "test1@test.com", GivenName = "Unit", FamilyName1 = "Test", Phone = "123456789" };
        var clientDto = new ClientDto { Id = 1, DocType = "nif", DocNum = "123456789P", Email = "test1@test.com", GivenName = "Unit", FamilyName1 = "Test", Phone = "123456789" };

        _mockClientRepository.Setup(repository => repository.GetById(It.IsAny<object>())).ReturnsAsync(client);
        _clientDtoValidator.Setup(validator => validator.Validate(It.IsAny<ClientDto>())).Returns(new FluentValidation.Results.ValidationResult());
        _clientMapper.Setup(mapper => mapper.Map<Client>(It.IsAny<ClientDto>())).Returns(client);

        // Act
        await _clientAppService.Delete(clientDto);

        // Assert
        _mockClientRepository.Verify(repository => repository.GetById(It.IsAny<object>()), Times.Once);
        _mockClientRepository.Verify(repository => repository.Delete(It.IsAny<Client>()), Times.Once);
        _clientDtoValidator.Verify(validator => validator.Validate(It.IsAny<ClientDto>()), Times.Once);
        _clientMapper.Verify(mapper => mapper.Map<Client>(It.IsAny<ClientDto>()), Times.Once);
    }
}