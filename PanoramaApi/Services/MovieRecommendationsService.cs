using Newtonsoft.Json;
using PanoramaApi.Gemini;
using PanoramaApi.Tmdb;
using PanoramaApi.Tmdb.Models;

namespace PanoramaApi.Services
{
    public class MovieRecommendationsService
    {
        public async Task<List<MovieResult>> FindSimilar(int tmdbId, int number)
        {
            var tmdb = new TmdbApi(new Uri(AppSettings.TmdbApiUrl), AppSettings.TmdbToken);
            var movie = await tmdb.GetMovie(tmdbId);

            return await FindSimilar(movie.Title, movie.ReleaseDate.Year, number);
        }

        public async Task<List<MovieResult>> FindSimilar(string title, int year, int number)
        {
            var gemini = new GeminiApi(new Uri(AppSettings.GeminiApiUrl), AppSettings.GeminiApiKey);

            var format = new
            {
                title = "Movie title 1",
                year = "Release year 1"
            };

            var prompt = @$"
                Write an array of {number} movies similar to ""{title}"" ({year}).
                Follow the pattern: [{JsonConvert.SerializeObject(format)}...]";

            var result = (await gemini.GenerateContent(prompt)).Candidates[0].Content.Parts[0].Text;
            var list = JsonConvert.DeserializeObject<List<GeminiMovieSearchResult>>(result);

            if (list == null)
            {
                throw new Exception("Error fetching results.");
            }

            return await SearchTmdb(list);
        }

        protected async Task<List<MovieResult>> SearchTmdb(List<GeminiMovieSearchResult> geminiResults)
        {
            var results = new List<MovieResult>();

            foreach (var geminiResult in geminiResults)
            {
                try
                {
                    var result = await TmdbHelper.FindMovie(geminiResult.Title, geminiResult.Year);

                    results.Add(result);
                }
                catch (NotFoundException) { }
            }

            return results;
        }
    }

    public class GeminiMovieSearchResult
    {
        public string Title { get; set; }
        public int Year { get; set; }
    }
}
