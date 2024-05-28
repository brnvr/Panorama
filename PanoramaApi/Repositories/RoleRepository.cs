using PanoramaApi.Models.Entities;

namespace PanoramaApi.Repositories
{
    public class RoleRepository : RepositoryBase<Role>
    {
        public RoleRepository(AppDbContext dbContext) : base("role", dbContext, dbContext => dbContext.Roles) { }

        public Role FindByUserId(int userId)
        {
            var query = from r in Entities
                        join u in DbContext.Users on r.Id equals u.RoleId
                        where u.Id == userId
                        select r;

            var role = query.FirstOrDefault();

            if (role == null)
            {
                throw new NotFoundException(EntityDescription, $"No role found for user {userId}.");
            }

            return role;
        }

        public Role FindByName(string name)
        {
            var query = from r in Entities
                        where r.Name == name
                        select r;

            var role = query.FirstOrDefault();

            if (role == null)
            {
                throw new NotFoundException(EntityDescription, $"Role \"{name}\" not found.");
            }

            return role;
        }

        public bool Exists(string name)
        {
            var roles = Entities.Where(r => r.Name == name).Select(x => new
            {
                x.Id
            });

            return roles.Count() > 0;
        }
    }
}
