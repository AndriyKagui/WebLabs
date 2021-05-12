using files_uploader_server.Helpers;
using files_uploader_server.Interfaces;
using files_uploader_server.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace files_uploader_server.Services
{
    public class FilesUploaderService : IFilesUploaderService
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly FileRequirements _fileRequirements;

        public FilesUploaderService(IWebHostEnvironment hostEnvironment, IOptions<FileRequirements> fileRequirements)
        {
            _hostingEnvironment = hostEnvironment;
            _fileRequirements = fileRequirements.Value;
        }

        /// <summary>
        /// Returns all files in directory
        /// </summary>
        /// <returns></returns>
        public List<FileViewModel> GetFilesMetaDataAsync()
        {
            var dir = new DirectoryInfo(_hostingEnvironment.ContentRootPath + @"\UploadedFiles");
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException("Directory not found");
            }
            var files = dir.GetFiles().Select(file => new FileViewModel
            {
                Name = file.Name.Replace(file.Extension, ""),
                Size = file.Length.ToMBytes(),
                UploadDate = file.LastWriteTime,
                Extention = file.Extension
            }).ToList();
            return files;
        }

        /// <summary>
        /// Saving file to directory
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task<FileViewModel> SaveFilesAsync(IFormFile file)
        {
            if (file is null)
            {
                throw new ArgumentNullException(nameof(file));
            }
            if(file.FileName.Replace(Path.GetExtension(file.FileName), "") == "")
            {
                throw new InvalidDataException("File must have name.");
            }
            if(file.Length > _fileRequirements.Size.ToBytes())
            {
                throw new InvalidOperationException("File is too large");
            }
            if (_fileRequirements.AllowedExtentions != null  && !_fileRequirements.AllowedExtentions.ToList().Contains(Path.GetExtension(file.FileName)))
            {
                throw new InvalidOperationException("Extention is not supported");
            }


            DateTime creationDate = DateTime.Now;
            if (file.Length > 0)
            {
                string folderPath = _hostingEnvironment.ContentRootPath + @"\UploadedFiles";
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                var path = Path.Combine(folderPath, file.FileName);

                using(var fileStream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                    creationDate = File.GetLastWriteTime(path);
                }
            }
            return new FileViewModel
            {
                Name = file.FileName.Replace(Path.GetExtension(file.FileName), ""),
                Size = file.Length,
                UploadDate = creationDate,
                Extention = Path.GetExtension(file.FileName)
            };
        }
    }
}
