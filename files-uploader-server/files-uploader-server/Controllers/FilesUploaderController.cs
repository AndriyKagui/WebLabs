using files_uploader_server.Helpers;
using files_uploader_server.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace files_uploader_server.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class FilesUploaderController : ControllerBase
    {
        private readonly IFilesUploaderService _service;
        private readonly ILogger _logger;

        public FilesUploaderController(IFilesUploaderService service, ILogger<FilesUploaderController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetFiles()
        {
            try
            {
                var files = _service.GetFilesMetaDataAsync();

                return new JsonResult(files);
            }
            catch(DirectoryNotFoundException e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, e.Message);
            }
            catch(Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Error while getting files.");
            }
        }

        [HttpPost]
        [RequestSizeLimit(GlobalHelper.RequestLimit)]
        [RequestFormLimits(MultipartBodyLengthLimit = GlobalHelper.RequestLimit)]
        public async Task<IActionResult> SaveFile(IFormFile file)
        {
            try
            {
                var fileResponse = await _service.SaveFilesAsync(file);

                return new JsonResult(fileResponse);
            }
            catch(InvalidOperationException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
            catch(InvalidDataException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
            catch(Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Error while saving file.");
            }
        }
    }
}
