using AutoMapper;
using Carglass.TechnicalAssessment.Backend.BL;
using Carglass.TechnicalAssessment.Backend.BL.Products;
using Carglass.TechnicalAssessment.Backend.DL.Repositories;
using Carglass.TechnicalAssessment.Backend.Dtos;
using Carglass.TechnicalAssessment.Backend.Entities;
using FluentValidation;
using Moq;

namespace Carglass.TechnicalAssessment.Backend.Implementation.UnitTests;

public class ProductAppServiceTests
{
    private readonly Mock<ICrudRepository<Product>> _mockProductRepository;
    private readonly Mock<IMapper> _productMapper;
    private readonly Mock<IValidator<ProductDto>> _productDtoValidator;
    private readonly ProductAppService _productAppService;

    public ProductAppServiceTests()
    {
        _mockProductRepository = new Mock<ICrudRepository<Product>>();
        _productMapper = new Mock<IMapper>();
        _productDtoValidator = new Mock<IValidator<ProductDto>>();
        _productAppService = new ProductAppService(_mockProductRepository.Object, _productMapper.Object, _productDtoValidator.Object);
    }

    [Fact]
    public async Task GetAll_ShouldReturnAllProducts()
    {
        // Arrange
        var products = new List<Product>
        {
            new Product { Id = 1, ProductName = "test1", ProductType = 1234, NumTerminal = 222222, SoldAt = "2021-10-03 12:33"},
            new Product { Id = 2, ProductName = "test2", ProductType = 4321, NumTerminal = 333333, SoldAt = "2021-03-10 21:12"}
        };

        var productsDto = new List<ProductDto>
        {
            new ProductDto { Id = 1, ProductName = "test1", ProductType = 1234, NumTerminal = 222222, SoldAt = "2021-10-03 12:33"},
            new ProductDto { Id = 2, ProductName = "test2", ProductType = 4321, NumTerminal = 333333, SoldAt = "2021-03-10 21:12"}
        };

        _mockProductRepository.Setup(repository => repository.GetAll()).ReturnsAsync(products);
        _productMapper.Setup(mapper => mapper.Map<IEnumerable<ProductDto>>(It.IsAny<IEnumerable<Product>>())).Returns(productsDto);

        // Act
        var result = await _productAppService.GetAll();

        // Assert
        Assert.Equal(productsDto, result);
    }

    [Fact]
    public async Task GetById_ShouldReturnAProduct()
    {
        // Arrange
        var product = new Product { Id = 1, ProductName = "test1", ProductType = 1234, NumTerminal = 222222, SoldAt = "2021-10-03 12:33" };
        var productDto = new ProductDto { Id = 1, ProductName = "test1", ProductType = 1234, NumTerminal = 222222, SoldAt = "2021-10-03 12:33" };

        _mockProductRepository.Setup(repository => repository.GetById(It.IsAny<object>())).ReturnsAsync(product);
        _productMapper.Setup(mapper => mapper.Map<ProductDto>(It.IsAny<Product>())).Returns(productDto);

        // Act
        var result = await _productAppService.GetById(product.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(productDto.Id, result.Id);
        Assert.Equal(productDto, result);
    }

    [Fact]
    public async Task Create_ShouldCreateProduct()
    {
        // Arrange
        var product = new Product { Id = 1, ProductName = "test1", ProductType = 1234, NumTerminal = 222222, SoldAt = "2021-10-03 12:33" };
        var productDto = new ProductDto { Id = 1, ProductName = "test1", ProductType = 1234, NumTerminal = 222222, SoldAt = "2021-10-03 12:33" };

        _productDtoValidator.Setup(validator => validator.Validate(It.IsAny<ProductDto>())).Returns(new FluentValidation.Results.ValidationResult());
        _productMapper.Setup(mapper => mapper.Map<Product>(It.IsAny<ProductDto>())).Returns(product);

        // Act
        await _productAppService.Create(productDto);

        // Assert
        _mockProductRepository.Verify(repository => repository.GetById(It.IsAny<object>()), Times.Once);
        _mockProductRepository.Verify(repository => repository.Create(product), Times.Once);
        _productDtoValidator.Verify(validator => validator.Validate(It.IsAny<ProductDto>()), Times.Once);
        _productMapper.Verify(mapper => mapper.Map<Product>(It.IsAny<ProductDto>()), Times.Once);
    }

    [Fact]
    public async Task Update_ShouldUpdateProduct()
    {
        // Arrange
        var product = new Product { Id = 1, ProductName = "test1", ProductType = 1234, NumTerminal = 222222, SoldAt = "2021-10-03 12:33" };
        var productDto = new ProductDto { Id = 1, ProductName = "test1", ProductType = 1234, NumTerminal = 222222, SoldAt = "2021-10-03 12:33" };

        _mockProductRepository.Setup(repository => repository.GetById(It.IsAny<object>())).ReturnsAsync(product);
        _productDtoValidator.Setup(validator => validator.Validate(It.IsAny<ProductDto>())).Returns(new FluentValidation.Results.ValidationResult());
        _productMapper.Setup(mapper => mapper.Map<Product>(It.IsAny<ProductDto>())).Returns(product);

        // Act
        await _productAppService.Update(productDto);

        // Assert
        _mockProductRepository.Verify(repository => repository.GetById(It.IsAny<object>()), Times.Once);
        _mockProductRepository.Verify(repository => repository.Update(product), Times.Once);
        _productDtoValidator.Verify(validator => validator.Validate(It.IsAny<ProductDto>()), Times.Once);
        _productMapper.Verify(mapper => mapper.Map<Product>(It.IsAny<ProductDto>()), Times.Once);
    }

    [Fact]
    public async Task DeleteById_ShouldDeleteProductById()
    {
        // Arrange
        var product = new Product { Id = 1, ProductName = "test1", ProductType = 1234, NumTerminal = 222222, SoldAt = "2021-10-03 12:33" };
        var productId = 1;

        _mockProductRepository.Setup(repository => repository.GetById(It.IsAny<object>())).ReturnsAsync(product);

        // Act
        await _productAppService.DeleteById(productId);

        // Assert
        _mockProductRepository.Verify(repository => repository.GetById(It.IsAny<object>()), Times.Once);
        _mockProductRepository.Verify(repository => repository.Delete(It.IsAny<Product>()), Times.Once);
    }

    [Fact]
    public async Task Delete_ShouldDeleteProduct()
    {
        // Arrange
        var product = new Product { Id = 1, ProductName = "test1", ProductType = 1234, NumTerminal = 222222, SoldAt = "2021-10-03 12:33" };
        var productDto = new ProductDto { Id = 1, ProductName = "test1", ProductType = 1234, NumTerminal = 222222, SoldAt = "2021-10-03 12:33" };

        _mockProductRepository.Setup(repository => repository.GetById(It.IsAny<object>())).ReturnsAsync(product);
        _productDtoValidator.Setup(validator => validator.Validate(It.IsAny<ProductDto>())).Returns(new FluentValidation.Results.ValidationResult());
        _productMapper.Setup(mapper => mapper.Map<Product>(It.IsAny<ProductDto>())).Returns(product);

        // Act
        await _productAppService.Delete(productDto);

        // Assert
        _mockProductRepository.Verify(repository => repository.GetById(It.IsAny<object>()), Times.Once);
        _mockProductRepository.Verify(repository => repository.Delete(It.IsAny<Product>()), Times.Once);
        _productDtoValidator.Verify(validator => validator.Validate(It.IsAny<ProductDto>()), Times.Once);
        _productMapper.Verify(mapper => mapper.Map<Product>(It.IsAny<ProductDto>()), Times.Once);
    }
}