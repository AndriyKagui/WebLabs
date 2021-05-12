using System.Collections.Generic;

namespace files_uploader_server.Models
{
    public class FileRequirements
    {
        public int Size { get; set; }
        public IEnumerable<string> AllowedExtentions { get; set; }
    }
}
