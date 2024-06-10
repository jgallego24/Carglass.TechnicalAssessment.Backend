using Carglass.TechnicalAssessment.Backend.DL.Database;
using Carglass.TechnicalAssessment.Backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace Carglass.TechnicalAssessment.Backend.DL.Repositories;

public class ClientIMRepository : IClientRepository
{
    //private ICollection<Client> _clients;
    private readonly ApplicationContext _context;

    public ClientIMRepository(ApplicationContext context)
    {
        //_clients = new HashSet<Client>()
        //{
        //    new Client()
        //    {
        //        Id = 1,
        //        DocType = "nif",
        //        DocNum = "11223344E",
        //        Email = "eromani@sample.com",
        //        GivenName = "Enriqueta",
        //        FamilyName1 = "Romani",
        //        Phone = "668668668"
        //    }
        //};
        _context = context;
    }

    public async Task<IEnumerable<Client>> GetAll()
    {
        return await _context.Clients.ToListAsync();
    }

    public async Task<Client?> GetById(params object[] keyValues)
    {
        return await _context.Clients.FindAsync(keyValues);
        //return _clients.SingleOrDefault(x => x.Id.Equals(keyValues[0]));
    }

    public async Task Create(Client item)
    {
        await _context.Clients.AddAsync(item);
        await _context.SaveChangesAsync();
        //_clients.Add(item);
    }

    public async Task Update(Client item)
    {
        //var toUpdateItem = _clients.Single(x => x.Id.Equals(item.Id));

        //toUpdateItem.Id = item.Id;
        //toUpdateItem.DocType = item.DocType;
        //toUpdateItem.DocNum = item.DocNum;
        //toUpdateItem.Email = item.Email;
        //toUpdateItem.GivenName = item.GivenName;
        //toUpdateItem.FamilyName1 = item.FamilyName1;
        //toUpdateItem.Phone = item.Phone;

        var existingClient = await _context.Clients.FindAsync(item.Id);
        if (existingClient is null)
        {
            throw new InvalidOperationException("El cliente facilitado no se encuentra en la base de datos");
        }

        existingClient.Id = item.Id;
        existingClient.DocType = item.DocType;
        existingClient.DocNum = item.DocNum;
        existingClient.Email = item.Email;
        existingClient.GivenName = item.GivenName;
        existingClient.FamilyName1 = item.FamilyName1;
        existingClient.Phone = item.Phone;

        await _context.SaveChangesAsync();
    }

    public async Task Delete(Client item)
    {
        //var toDeleteItem = _clients.Single(x => x.Id.Equals(item.Id));

        //_clients.Remove(toDeleteItem);

        var itemToDelete = await _context.Clients.FindAsync(item.Id);

        if (itemToDelete is not null)
        {
            _context.Clients.Remove(itemToDelete);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> DocNumExists(string docNum)
    {
        return await _context.Clients.AnyAsync(x => x.DocNum.Equals(docNum));
    }
}
