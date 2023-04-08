namespace DietPlanner.DAL.Repositories.Abstract.Base;

public interface IGenericRepository<TKey, TEntity>
{
    TEntity Add(TEntity entity);
    bool Delete(TKey key);
    bool Update(TEntity entity);
    List<TEntity> GetAll(Func<TEntity, bool> predicate);
    TEntity GetById(TKey key);
}
