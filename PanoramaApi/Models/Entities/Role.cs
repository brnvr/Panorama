namespace PanoramaApi.Models.Entities
{
    public class Role : IEntity
    {
        public int Id { get; private set; }
        public string Name { get; set; }
        public string Description { get; set; } 
    }
}
