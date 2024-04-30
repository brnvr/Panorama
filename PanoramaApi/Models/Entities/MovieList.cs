namespace PanoramaApi.Models.Entities
{
    public class MovieList : IEntity
    {
        public int Id { get; private set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public int UserId { get; set; } 
        public DateTime CreationDate { get; set; }
    }
}
