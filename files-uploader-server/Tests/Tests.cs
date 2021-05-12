using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using files_uploader_server.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using files_uploader_server.Models;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Tests
{
    public class Tests
    {
        private readonly FilesUploaderService service;
        private readonly Mock<IWebHostEnvironment> env = new Mock<IWebHostEnvironment>();

        public Tests()
        {
            var requirements = Options.Create(new FileRequirements());
            requirements.Value.Size = 10;
            requirements.Value.AllowedExtentions = new List<string>()
            {
                ".txt"
            };
            service = new FilesUploaderService(env.Object, requirements);
        }
        [Fact]
        public async Task NormalBehaviorTest()
        {
            var file = new Mock<IFormFile>();
            file.Setup(x => x.FileName).Returns("Test.txt");
            file.Setup(x => x.Length).Returns(5*1024*1024);
            var result = await service.SaveFilesAsync(file.Object);

            Assert.Equal("Test", result.Name);
            Assert.Equal(".txt", result.Extention);
        }

        [Fact]
        public async Task TooLargeFileTest()
        {
            var file = new Mock<IFormFile>();
            file.Setup(x => x.FileName).Returns("Test.txt");
            file.Setup(x => x.Length).Returns(50 * 1024 * 1024);
            var exception = await Assert.ThrowsAsync<Exception>(() => service.SaveFilesAsync(file.Object));
            Assert.Equal("File is too large", exception.Message);
        }

        [Fact]
        public async Task UnsupportedTypeTest()
        {
            var file = new Mock<IFormFile>();
            file.Setup(x => x.FileName).Returns("Test.json");
            file.Setup(x => x.Length).Returns(5 * 1024 * 1024);
            var exception = await Assert.ThrowsAsync<Exception>(() => service.SaveFilesAsync(file.Object));
            Assert.Equal("Extention is not supported", exception.Message);
        }
    }
}
