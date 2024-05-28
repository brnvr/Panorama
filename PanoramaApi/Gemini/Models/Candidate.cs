namespace PanoramaApi.Gemini.Models
{
    public class Candidate
    {
        public ResponseContent Content { get; set; }
        public string FinishReason { get; set; }
        public int Index { get; set; }
    }
}
