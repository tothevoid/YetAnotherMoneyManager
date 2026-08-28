using System.IO;

namespace Audex.Application.DTO.FileStorage
{
    public class FileStreamDto
    {
        public Stream Stream { get; set; }
        public string ContentType { get; set; }
    }
}
