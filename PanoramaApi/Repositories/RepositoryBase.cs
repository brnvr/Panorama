using Microsoft.EntityFrameworkCore;
using PanoramaApi.Models.Entities;

namespace PanoramaApi.Repositories
{
    public abstract class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        public string EntityDescription { get; }
        protected AppDbContext DbContext { get; }
        protected DbSet<TEntity> Entities { get; }

        public RepositoryBase(string entityDescription, AppDbContext dbContext, Func<AppDbContext, DbSet<TEntity>> entitiesFromContext)
        {
            Entities = entitiesFromContext(dbContext);
            DbContext = dbContext;
            EntityDescription = entityDescription;
        }

        public virtual List<TEntity> Find(Func<DbSet<TEntity>, IEnumerable<TEntity>>? filter = null)
        {
            if (filter == null)
            {
                return Entities.ToList();
            }

            return filter(Entities).ToList();
        }

        public virtual TEntity FindById(int id)
        {
            var entity = Entities.Find(id);

            if (entity == null)
            {
                throw new NotFoundException(EntityDescription, $"{EntityDescription} not found with id {id}.");
            }

            return entity;
        }

        public virtual void Add(TEntity entity)
        {
            Entities.Add(entity);
        }

        public virtual TEntity Update(int id, Action<TEntity> updateFunction)
        {
            var entity = FindById(id);

            updateFunction(entity);

            return entity;
        }

        public virtual void Remove(int id)
        {
            Entities.Remove(FindById(id));
        }

        public virtual bool Exists(int id)
        {
            var results = Entities.Where(x => x.Id == id).Select(x => new
            {
                x.Id
            });

            return results.Count() > 0;
        }
    }
}
