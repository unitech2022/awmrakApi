using System;
using System.IO;
using AutoMapper;
using awamrakeApi.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace awamrakeApi.Controllers
{

    [Route("image")]
    [ApiController]
    public class ImageController : ControllerBase
    {


        private IWebHostEnvironment _hostingEnvironment;

        public ImageController(IWebHostEnvironment hostingEnvironment)
        {

            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost]
        [Route("upload/car")]
        public ActionResult uploadImage([FromForm] IFormFile file)
        {
            string path = _hostingEnvironment.WebRootPath + "/images/";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            String fileName = DateTime.Now.ToString("yyyyMMddTHHmmss") + ".jpeg";
            using (var fileStream = System.IO.File.Create(path + fileName))
            {
                file.CopyTo(fileStream);
                fileStream.Flush();
                return Ok(fileName);
            }
        }

    }
}