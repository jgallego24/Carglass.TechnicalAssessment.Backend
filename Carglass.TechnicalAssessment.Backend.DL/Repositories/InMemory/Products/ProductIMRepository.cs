using Carglass.TechnicalAssessment.Backend.DL.Database;
using Carglass.TechnicalAssessment.Backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace Carglass.TechnicalAssessment.Backend.DL.Repositories;

public class ProductIMRepository : ICrudRepository<Product>
{
    private readonly ApplicationContext _context;

    public ProductIMRepository(ApplicationContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Product>> GetAll()
    {
        return await _context.Products.ToListAsync();
    }

    public async Task<Product?> GetById(params object[] keyValues)
    {
        return await _context.Products.FindAsync(keyValues);
    }

    public async Task Create(Product item)
    {
        await _context.Products.AddAsync(item);
        await _context.SaveChangesAsync();
    }
    public async Task Update(Product item)
    {
        var existingProduct = await _context.Products.FindAsync(item.Id);
        if (existingProduct is null)
        {
            throw new InvalidOperationException("El producto facilitado no se encuentra en la base de datos");
        }

        existingProduct.Id = item.Id;
        existingProduct.ProductName = item.ProductName;
        existingProduct.ProductType = item.ProductType;
        existingProduct.NumTerminal = item.NumTerminal;
        existingProduct.SoldAt = item.SoldAt;

        await _context.SaveChangesAsync();
    }

    public async Task Delete(Product item)
    {
        var itemToDelete = await _context.Products.FindAsync(item.Id);

        if (itemToDelete is not null)
        {
            _context.Products.Remove(itemToDelete);
            await _context.SaveChangesAsync();
        }
    }
}
