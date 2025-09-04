
namespace FIAPOracleEF.Database;

public class GenericRepository<T> where T : class
{
    private readonly AppDbContext _context;

    public GenericRepository(AppDbContext context)
    {
        _context = context;
    }

    public List<T> GetAll()
    {
        return _context.Set<T>().ToList();
    }

    public T? GetById(int id)
    {
        return _context.Set<T>().Find(id);
    }

    public int Insert(T entity)
    {
        _context.Set<T>().Add(entity);
        return _context.SaveChanges();
    }

    public bool Update(T entity)
    {
        _context.Set<T>().Update(entity);
        return (_context.SaveChanges() > 0);
    }

    public bool Delete(T entry)
    {
        _context.Set<T>().Remove(entry);
        return (_context.SaveChanges() > 0);
    }

    public bool DeleteById(int id)
    {
        throw new NotImplementedException();
    }

    public List<T> SearchBy(Func<T, bool> condition)
    {
        throw new NotImplementedException();
    }
}
