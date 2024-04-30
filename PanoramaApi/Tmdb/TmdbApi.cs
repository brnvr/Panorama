using PanoramaApi.Tmdb.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace PanoramaApi.Tmdb
{
    public class TmdbApi
    {
        public Uri Url { get; }
        public string Token { get; }

        public TmdbApi(Uri url, string token)
        {
            Url = url;
            Token = token;
        }

        public async Task<SearchResults<MovieResult>> SearchMovies(string query, int page)
        {
            return await MakeRequest<SearchResults<MovieResult>>("3/search/movie", client => client.GetAsync(""), new Dictionary<string, object>
            {
                { "page", page },
                { "query", query }
            });
        }

        public async Task<MovieDetails> GetMovie(int id)
        {
            return await MakeRequest<MovieDetails>($"3/movie/{id}", client => client.GetAsync(""));
        }

        protected async Task<T> MakeRequest<T>(string path, Func<HttpClient, Task<HttpResponseMessage>> callback, Dictionary<string, object>? query = null)
        {
            using (var client = new HttpClient())
            {
                var queryItems = query is null ? new List<string>() : query.Select(pair => $"{pair.Key}={pair.Value}");

                client.BaseAddress = new Uri($"{Url}{path}?{string.Join('&', queryItems)}");
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {Token}");

                var result = await callback(client);
                var content = await result.Content.ReadAsStringAsync();

                if (result.IsSuccessStatusCode)
                {
                    var obj = JsonConvert.DeserializeObject<T>(content, new JsonSerializerSettings
                    {
                        ContractResolver = new DefaultContractResolver
                        {
                            NamingStrategy = new SnakeCaseNamingStrategy() 
                        }
                    });

                    if (obj is null)
                    {
                        throw new Exception("Request return nothing.");
                    }

                    return obj;
                }

                throw new HttpRequestException(content, null, result.StatusCode);
            }
        }
    }
}
