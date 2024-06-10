using AutoMapper;
using Carglass.TechnicalAssessment.Backend.Dtos;
using Carglass.TechnicalAssessment.Backend.Entities;

namespace Carglass.TechnicalAssessment.Backend.BL.Products.Converter;

public class ProductProfileConverter : Profile
{
    public ProductProfileConverter()
    {
        // Entity to Dto
        CreateMap<Product, ProductDto>();

        // Dto to Entity
        CreateMap<ProductDto, Product>();
    }
}
