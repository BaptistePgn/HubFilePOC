using System.Xml;

namespace HubFile.Models
{
    public class FileModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Size { get; set; }
        public string Extension { get; set; }
    }
}
