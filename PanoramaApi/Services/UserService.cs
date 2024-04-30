using PanoramaApi.Enums;
using PanoramaApi.Models.Entities;
using PanoramaApi.Models.View;
using PanoramaApi.Repositories;
using PanoramaApi.Tmdb;
using PanoramaApi.Tmdb.Models;
using System;

namespace PanoramaApi.Services
{
    public class UserService
    {
        readonly AppDbContext _dbContext;
        readonly static string[] _validPictureExtensions = new string[] { ".jpeg", ".jpg", ".png" };
        const long _pictureFileSizeLimit = 10 * 1024 * 1024;

    public UserService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int Register(Registration registration)
        {
            var user = new User
            {
                Username = registration.Username,
                Email = registration.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(registration.Password),
                RoleId = new RoleRepository(_dbContext).FindByName(Roles.Standard).Id,
                RegistrationDate = DateTime.Now.ToUniversalTime()
            };

            new UserRepository(_dbContext).Add(user);

            _dbContext.SaveChanges();

            return user.Id;
        }

        public void EditPassword(int userId, string newPassword)
        {
            var repo = new UserRepository(_dbContext);

            repo.Update(userId, user => BCrypt.Net.BCrypt.HashPassword(newPassword));

            _dbContext.SaveChanges();
        }

        public async Task AddMovie(int userId, int tmdbMovieId)
        {
            await new TmdbApi(new Uri(AppSettings.TmdbApiUrl), AppSettings.TmdbToken).GetMovie(tmdbMovieId);

            new WatchedMovieRepository(_dbContext).Add(new WatchedMovie
            {
                UserId = userId,
                TmdbId = tmdbMovieId
            });

            _dbContext.SaveChanges();
        }

        public List<int> GetMovies(int userId)
        {
            return new WatchedMovieRepository(_dbContext).Find(movies => movies.Where(movie => movie.UserId == userId)).Select(movie => movie.TmdbId).ToList();
        }

        public async Task<List<MovieDetails>> GetMoviesDetails(int userId)
        {
            var movies = new WatchedMovieRepository(_dbContext).Find(movies => movies.Where(movie => movie.UserId == userId)).ToList();
            var tmdbApi = new TmdbApi(new Uri(AppSettings.TmdbApiUrl), AppSettings.TmdbToken);

            var details = new List<MovieDetails>();

            foreach(var movie in movies)
            {
                details.Add(await tmdbApi.GetMovie(movie.TmdbId));
            }

            return details;
        }

        public void RemoveMovie(int userId, int tmdbMovieId)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                var repo = new WatchedMovieRepository(_dbContext);

                var watchedMovie = repo.Find(userId, tmdbMovieId);

                repo.Remove(watchedMovie.Id);

                _dbContext.SaveChanges();
                transaction.Commit();
            }
        }

        public async Task<string> EditPicture(int userId, IFormFile file)
        {
            if (file.Length > _pictureFileSizeLimit)
            {
                throw new ArgumentException($"Picture can't be larger than {_pictureFileSizeLimit}mb (real: {file.Length}).");
            }

            var filePath = GeneratePicturePath(file);

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                var repo = new UserRepository(_dbContext);
                var user = repo.FindById(userId);   

                if (!string.IsNullOrEmpty(user.PicturePath))
                {
                    RemovePicture(user.PicturePath);
                }

                Directory.CreateDirectory(Path.Combine("images", "profile_pictures"));

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                repo.Update(userId, user => user.PicturePath = filePath);
                _dbContext.SaveChanges();
                transaction.Commit();

                return filePath;
            }

            void RemovePicture(string filePath)
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }

            static string GeneratePicturePath(IFormFile file)
            {
                var extension = Path.GetExtension(file.FileName);

                if (!_validPictureExtensions.Contains(extension))
                {
                    throw new ArgumentException($"Invalid format. Accepted: {string.Join(", ", _validPictureExtensions)}, real: {extension}.");
                }

                var fileName = Guid.NewGuid().ToString().Replace("-", "") + extension;
                return Path.Combine("images", "profile_pictures", fileName);
            }
        }    
    }
}