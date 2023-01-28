namespace Bootstrap_3.ViewModel
{
    public class CreateProjectVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IFormFile ImageUrl { get; set; }
        public string Description { get; set; }
        public string? Author { get; set; }
    }
}
