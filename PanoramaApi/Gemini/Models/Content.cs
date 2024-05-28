namespace PanoramaApi.Gemini.Models
{
    public class Content
    {
        public List<ContentPart> Parts { get; set; }
    }

    public class ResponseContent : Content
    {
        public string Role { get; set; }
    }
}
