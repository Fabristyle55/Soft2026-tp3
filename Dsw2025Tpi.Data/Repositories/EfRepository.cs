using Dsw2025Tpi.Domain.Entities;
using Dsw2025Tpi.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols;
using System.Linq.Expressions;

namespace Dsw2025Tpi.Data.Repositories;

public class EfRepository: IRepository
{
    private readonly Dsw2025TpiContext _context;
    //private List<Product> _products;

    public EfRepository(Dsw2025TpiContext context)
    {
        _context = context;
    }

    /*
    public EfRepository()
    {
        // This constructor is intentionally left empty for dependency injection purposes.
    }

    private void LoadProducts()
    {
        var json = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Sources\\Products.json");
        _products = System.Text.Json.JsonSerializer.Deserialize<List<Product>>(json, new System.Text.Json.JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase
        }) ?? new List<Product>();
    }*/

    public async Task<T> Add<T>(T entity) where T : EntityBase
    {
        await _context.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<T> Delete<T>(T entity) where T : EntityBase
    {
        _context.Remove(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<T?> First<T>(Expression<Func<T, bool>> predicate, params string[] include) where T : EntityBase
    {
        return await Include(_context.Set<T>(), include).FirstOrDefaultAsync(predicate);
    }

    public async Task<IEnumerable<T>?> GetAll<T>(params string[] include) where T : EntityBase
    {
        return await Include(_context.Set<T>(), include).ToListAsync();
    }

    public async Task<T?> GetById<T>(Guid id, params string[] include) where T : EntityBase
    {
        return await Include(_context.Set<T>(), include).FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<IEnumerable<T>?> GetFiltered<T>(Expression<Func<T, bool>> predicate, params string[] include) where T : EntityBase
    {
        return await Include(_context.Set<T>(), include).Where(predicate).ToListAsync();
    }

    public async Task<T> Update<T>(T entity) where T : EntityBase
    {
        _context.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    private static IQueryable<T> Include<T>(IQueryable<T> query, string[] includes) where T : EntityBase
    {
        var includedQuery = query;

        foreach (var include in includes)
        {
            includedQuery = includedQuery.Include(include);
        }
        return includedQuery;
    }
}
