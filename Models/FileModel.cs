namespace HubFile.Models
{
    public class FileModel
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Path { get; set; }
        public required string Size { get; set; }
        public required string Extension { get; set; }
    }
}
