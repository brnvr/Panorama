using PanoramaApi.Models.Entities;
using PanoramaApi.Repositories;
using PanoramaApi.Tmdb;
using PanoramaApi.Tmdb.Models;
using System.Text.RegularExpressions;

namespace PanoramaApi.Services
{
    public class MovieListService
    {
        AppDbContext _dbContext;

        public MovieListService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<MovieList> FindList(int userId, string query)
        {
            return new MovieListRepository(_dbContext).Find(e =>
            {
                return e.Where(l => l.UserId == userId && l.Title.ToUpper().Contains(query.ToUpper()));
            });
        }

        public string CreateList(int userId, string title, string description)
        {
            var repo = new MovieListRepository(_dbContext);

            var list = new MovieList
            {
                Title = title.Trim(),
                Name = GenerateNameFromTitle(title),
                Description = description.Trim(),
                CreationDate = DateTime.Now.ToUniversalTime(),
                UserId = userId
            };

            repo.Add(list);

            _dbContext.SaveChanges();

            return list.Name;
        }

        public void RemoveList(int userId, string name)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                var repo = new MovieListRepository(_dbContext);
                var list = repo.Find(userId, name);

                repo.Remove(list.Id);

                _dbContext.SaveChanges();
                transaction.Commit();
            }
        }
        
        public MovieList EditList(int userId, string name, Action<MovieList> updateFunction)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                var repo = new MovieListRepository(_dbContext);
                var list = repo.Find(userId, name);
                var previousTitle = list.Title;

                repo.Update(list.Id, updateFunction);
                repo.Update(list.Id, l => l.Name = GenerateNameFromTitle(list.Title));

                _dbContext.SaveChanges();
                transaction.Commit();

                return list;
            }
        }

        public void DeleteList(int userId, string name)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                var repo = new MovieListRepository(_dbContext);

                var list = repo.Find(userId, name);

                repo.Remove(list.Id);

                _dbContext.SaveChanges();

                transaction.Commit();
            }
        }

        public async Task<List<MovieDetails>> GetMovies(int userId, string listName)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                var list = new MovieListRepository(_dbContext).Find(userId, listName);

                var entries = new MovieListEntryRepository(_dbContext)
                    .Find(entries => entries
                    .Where(entry => entry.Id == list.Id)
                    .OrderBy(entry => entry.Id));

                var tmdbApi = new TmdbApi(new Uri(AppSettings.TmdbApiUrl), AppSettings.TmdbToken);
                var movies = new List<MovieDetails>();

                foreach (var entry in entries)
                {
                    movies.Add(await tmdbApi.GetMovie(entry.TmdbId));
                }

                return movies;
            }
        }

        public async Task<int> AddMovie(int userId, string listName, int tmdbMovieId)
        {
            var tmdbApi = new TmdbApi(new Uri(AppSettings.TmdbApiUrl), AppSettings.TmdbToken);

            var movie = await tmdbApi.GetMovie(tmdbMovieId);

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                var repo = new MovieListEntryRepository(_dbContext);
                var list = new MovieListRepository(_dbContext).Find(userId, listName);

                var entry = new MovieListEntry
                {
                    ListId = list.Id,
                    TmdbId = tmdbMovieId       
                };

                repo.Add(entry);

                _dbContext.SaveChanges();
                transaction.Commit();

                return entry.Id;
            }
        }

        public async void RemoveMovie(int userId, string listName, int tmdbMovieId)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                var repo = new MovieListEntryRepository(_dbContext);
                var list = new MovieListRepository(_dbContext).Find(userId, listName);

                var entry = repo.Find(list.Id, tmdbMovieId);

                repo.Remove(entry.Id);

                _dbContext.SaveChanges();
                transaction.Commit();
            }
        }

        protected string GenerateNameFromTitle(string title)
        {
            string pattern = @"[^a-zA-Z0-9-]";

            return Regex.Replace(title.ToLower().Trim().Replace(" ", "-"), pattern, "");
        }
    }
}
