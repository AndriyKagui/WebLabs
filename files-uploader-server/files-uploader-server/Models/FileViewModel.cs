using System;

namespace files_uploader_server.Models
{
    public class FileViewModel
    {
        public string Name { get; set; }
        public DateTime UploadDate { get; set; }
        public double Size { get; set; }
        public string Extention { get; set; }
    }
}
