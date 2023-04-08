using DietPlanner.DAL.Context;
using DietPlanner.DAL.Repositories.Abstract.Base;

using Microsoft.EntityFrameworkCore;

namespace Math.DAL.Repository.Base;

public abstract class GenericRepository<TKey, TEntity> : IGenericRepository<TKey, TEntity> where TEntity : class
{
    private readonly DietPlannerContext _context;

    public DbSet<TEntity> DbSet => _context.Set<TEntity>();

    protected GenericRepository(DietPlannerContext dbContext)
    {
        _context = dbContext;
    }

    public TEntity Add(TEntity entity)
    {
        var item = DbSet.Add(entity).Entity;
        // _context.SaveChanges();
        return item;
    }

    public bool Delete(TKey key)
    {
        TEntity item = DbSet.Find(key);
        if (item == null)
        {
            return false;
        }

        DbSet.Remove(item);
        // _context.SaveChanges();
        return true;
    }

    public virtual bool Update(TEntity entity)
    {
        if (entity == null)
        {
            return false;
        }

        // DbSet.Find(entity.)

        DbSet.Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
        // _context.SaveChanges();
        return true;
    }

    public virtual List<TEntity> GetAll(Func<TEntity, bool> predicate)
    {
        List<TEntity> items = DbSet.Where(predicate).ToList();
        return items;
    }

    public virtual TEntity GetById(TKey key)
    {
        TEntity item = DbSet.Find(key);
        return item;
    }
}
