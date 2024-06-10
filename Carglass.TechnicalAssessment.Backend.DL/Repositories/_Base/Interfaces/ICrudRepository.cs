using Carglass.TechnicalAssessment.Backend.Entities;

namespace Carglass.TechnicalAssessment.Backend.DL.Repositories;

public interface ICrudRepository<TEntity>
{
    Task<IEnumerable<TEntity>> GetAll();
    Task<TEntity?> GetById(params object[] keyValues);

    Task Create(TEntity item);
    Task Update(TEntity item);
    Task Delete(TEntity item);
}


public interface IClientRepository : ICrudRepository<Client>
{
    Task<bool> DocNumExists(string docNum);
}