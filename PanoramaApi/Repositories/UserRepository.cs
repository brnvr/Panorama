using PanoramaApi.Models.Entities;

namespace PanoramaApi.Repositories
{
    public class UserRepository : RepositoryBase<User>
    {
        public UserRepository(AppDbContext dbContext) : base("user", dbContext, dbContext => dbContext.Users) { }

        public bool EmailExists(string email)
        {
            var query = from u in Entities where u.Email == email select new { u.Id };
            var user = query.FirstOrDefault();

            return user != null;
        }

        public bool UsernameExists(string username)
        {
            var query = from u in Entities where u.Username == username select new { u.Id };
            var user = query.FirstOrDefault();

            return user != null;
        }

        public User FindByUsernameOrEmail(string username)
        {
            var query = from u in Entities where u.Username == username || u.Email == username select u;
            var user = query.FirstOrDefault();

            if (user == null)
            {
                throw new EntityNotFoundException(EntityDescription, $"User not found with username or email {username}.");
            }

            return user;
        }
    }
}