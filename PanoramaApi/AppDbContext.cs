using Microsoft.EntityFrameworkCore;
using PanoramaApi.Models.Entities;
using PanoramaApi.Repositories;
using Npgsql;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace PanoramaApi
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base() { }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<WatchedMovie> WatchedMovies { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<MovieList> MovieLists { get; set; }
        public DbSet<MovieListEntry> MovieListEntries { get; set; }

        public void Initialize()
        {
            var roleRepository = new RoleRepository(this);

            if (!roleRepository.Exists("standard"))
            {
                roleRepository.Add(new Role
                {
                    Name = "standard",
                    Description = "Standard users"
                });
            }

            if (!roleRepository.Exists("system_admin"))
            {
                roleRepository.Add(new Role
                {
                    Name = "system_admin",
                    Description = "System admin"
                });
            }

            SaveChanges();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dataSourceBuilder = new NpgsqlDataSourceBuilder(AppSettings.ConnectionString);
            var dataSource = dataSourceBuilder.Build();

            optionsBuilder.UseSnakeCaseNamingConvention();
            optionsBuilder.UseNpgsql(dataSource);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            OnUserModelCreating(modelBuilder.Entity<User>());
            OnRoleModelCreating(modelBuilder.Entity<Role>());
            OnWatchedMovieModelCreating(modelBuilder.Entity<WatchedMovie>());
            OnReviewModelCreating(modelBuilder.Entity<Review>());
            OnMovieListModelCreating(modelBuilder.Entity<MovieList>());
        }

        protected void OnUserModelCreating(EntityTypeBuilder<User> entity)
        {
            entity.Property(user => user.Email).HasMaxLength(320);
            entity.Property(user => user.Username).HasMaxLength(32);
            entity.Property(user => user.Password).IsFixedLength().HasMaxLength(60);
            entity.HasIndex(user => user.Email).IsUnique();
            entity.HasIndex(user => user.Username).IsUnique();
            entity.HasOne<Role>().WithMany().HasForeignKey(user => user.RoleId);
        }

        protected void OnRoleModelCreating(EntityTypeBuilder<Role> entity)
        {
            entity.HasIndex(role => role.Name).IsUnique();
            entity.Property(role => role.Name).HasMaxLength(32);
            entity.Property(role => role.Description).HasMaxLength(180);
        }

        public void OnWatchedMovieModelCreating(EntityTypeBuilder<WatchedMovie> entity)
        {
            entity.HasOne<User>().WithMany().HasForeignKey(movie => movie.UserId);
            entity.HasIndex(movie => new { movie.UserId, movie.TmdbId }).IsUnique();
        }

        public void OnReviewModelCreating(EntityTypeBuilder<Review> entity)
        {
            entity.HasOne<User>().WithMany().HasForeignKey(review => review.UserId);
            entity.HasIndex(review => new { review.UserId, review.TmdbId }).IsUnique();
            entity.Property(review => review.Rating).HasMaxLength(2);
        }

        public void OnMovieListModelCreating(EntityTypeBuilder<MovieList> entity)
        {
            entity.HasIndex(list => new { list.UserId, list.Name }).IsUnique();
            entity.HasOne<User>().WithMany().HasForeignKey(list => list.UserId);
            entity.Property(list => list.Title).HasMaxLength(32);
            entity.Property(list => list.Description).HasMaxLength(500);
        }
        
        public void OnMovieListEntryModelCreating(EntityTypeBuilder<MovieListEntry> entity)
        {
            entity.HasIndex(movie => new { movie.ListId, movie.TmdbId }).IsUnique();
            entity.HasOne<MovieList>().WithMany().HasForeignKey(movie => movie.ListId);
        }
    }
}
