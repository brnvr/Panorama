namespace PanoramaApi.Gemini.Models
{
    public class GenerateContentResponse
    {
        public List<Candidate> Candidates { get; set; }
        public UsageMetadata UsageMetadata { get; set; }
    }
}
