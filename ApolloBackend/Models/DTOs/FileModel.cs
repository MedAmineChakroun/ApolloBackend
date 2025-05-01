namespace ApolloBackend.Models.DTOs
{
    public class FileModel
    {
        public string FileName { get; set; }
        public IFormFile file { get; set; }
    }
}
