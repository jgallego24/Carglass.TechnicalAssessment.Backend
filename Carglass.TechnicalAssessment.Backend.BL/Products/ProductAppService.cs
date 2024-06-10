using AutoMapper;
using Carglass.TechnicalAssessment.Backend.DL.Repositories;
using Carglass.TechnicalAssessment.Backend.Dtos;
using Carglass.TechnicalAssessment.Backend.Entities;
using FluentValidation;

namespace Carglass.TechnicalAssessment.Backend.BL.Products;

public class ProductAppService : IProductAppService
{
    private readonly ICrudRepository<Product> _productRepository;
    private readonly IMapper _productMapper;
    private readonly IValidator<ProductDto> _productDtoValidator;

    public ProductAppService(ICrudRepository<Product> clientRepository, IMapper productMapper, IValidator<ProductDto> productDtoValidator)
    {
        _productRepository = clientRepository;
        _productMapper = productMapper;
        _productDtoValidator = productDtoValidator;
    }

    public async Task<IEnumerable<ProductDto>> GetAll()
    {
        var products = await _productRepository.GetAll();
        return _productMapper.Map<IEnumerable<ProductDto>>(products);
    }

    public async Task<ProductDto> GetById(params object[] keyValues)
    {
        var client = await _productRepository.GetById(keyValues);
        return _productMapper.Map<ProductDto>(client);
    }

    public async Task Create(ProductDto productDto)
    {
        if (null != await _productRepository.GetById(productDto.Id))
        {
            throw new Exception("Ya existe un producto con este Id");
        }

        ValidateDto(productDto);

        var newProduct = _productMapper.Map<Product>(productDto);
        await _productRepository.Create(newProduct);
    }
    public async Task Update(ProductDto productDto)
    {
        if (null == await _productRepository.GetById(productDto.Id))
        {
            throw new Exception("No existe ningún cliente con este Id");
        }

        ValidateDto(productDto);

        var entity = _productMapper.Map<Product>(productDto);
        await _productRepository.Update(entity);
    }

    public async Task DeleteById(int productId)
    {
        var clientToDelete = await _productRepository.GetById(productId) ?? throw new Exception("No existe ningún cliente con este Id");

        await _productRepository.Delete(clientToDelete);
    }

    public async Task Delete(ProductDto productDto)
    {
        if (null == _productRepository.GetById(productDto.Id))
        {
            throw new Exception("No existe ningún cliente con este Id");
        }

        ValidateDto(productDto);

        var productToDelete = _productMapper.Map<Product>(productDto);

        await _productRepository.Delete(productToDelete);
    }

    private void ValidateDto(ProductDto productDto)
    {
        var validationResult = _productDtoValidator.Validate(productDto);
        if (validationResult.Errors.Any())
        {
            string toShowErrors = string.Join("; ", validationResult.Errors.Select(s => s.ErrorMessage));
            throw new Exception($"El producto especificado no cumple los requisitos de validación. Errores: '{toShowErrors}'");
        }
    }
}
