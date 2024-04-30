namespace PanoramaApi.Tmdb.Models
{
    public class SearchResults<TResult>
    {
        public int Page { get; set; }
        public List<TResult> Results { get; set; }
    }
}
