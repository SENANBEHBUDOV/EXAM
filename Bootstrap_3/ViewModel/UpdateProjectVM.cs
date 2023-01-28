namespace Bootstrap_3.ViewModel
{
    public class UpdateProjectVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IFormFile ImageUrl { get; set; }
        public string Description { get; set; }
        public string? Author { get; set; }
    }
}
