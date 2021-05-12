using files_uploader_server.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace files_uploader_server.Interfaces
{
    public interface IFilesUploaderService
    {
        public Task<FileViewModel> SaveFilesAsync(IFormFile files);
        public List<FileViewModel> GetFilesMetaDataAsync();
    }
}
