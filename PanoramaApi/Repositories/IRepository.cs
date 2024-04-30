using Microsoft.EntityFrameworkCore;
using PanoramaApi.Models.Entities;

namespace PanoramaApi.Repositories
{
    public interface IRepository<TEntity> where TEntity : class, IEntity
    {
        TEntity FindById(int id);
        List<TEntity> Find(Func<DbSet<TEntity>, IEnumerable<TEntity>>? filter);
        void Add(TEntity entity);
        TEntity Update(int id, Action<TEntity> updateFunction);
        void Remove(int id);
    }
}
