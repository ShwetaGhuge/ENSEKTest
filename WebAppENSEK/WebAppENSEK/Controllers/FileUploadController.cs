using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAppENSEK.Services;

namespace WebAppENSEK.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        private readonly IRepository _repository;

        public FileUploadController(IRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult MeterReadingUploads(IFormFile file)
        {
            var result = _repository.MeterReadingUpload(file);
            return Ok(result);
        }
    }
}
