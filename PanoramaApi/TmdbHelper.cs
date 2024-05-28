using PanoramaApi.Tmdb.Models;
using PanoramaApi.Tmdb;

namespace PanoramaApi
{
    public static class TmdbHelper
    {
        public static async Task<MovieResult> FindMovie(string title, int year)
        {
            var tmdb = new TmdbApi(new Uri(AppSettings.TmdbApiUrl), AppSettings.TmdbToken);
            var movies = await tmdb.SearchMovies(title, 1, year);

            if (movies.Results.Count == 0)
            {
                throw new NotFoundException("movie", $"No movie found with title \"{title}\" and release year {year}.");
            }

            return movies.Results[0];
        }
    }
}
