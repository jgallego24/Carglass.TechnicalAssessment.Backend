namespace Carglass.TechnicalAssessment.Backend.BL;

public interface ICrudAppService<TDto>
{
    Task<IEnumerable<TDto>> GetAll();
    Task<TDto> GetById(params object[] keyValues);

    Task Create(TDto item);
    Task Update(TDto item);
    Task Delete(TDto item);
    Task DeleteById(int id);
}
