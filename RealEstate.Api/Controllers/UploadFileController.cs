
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstate.API.Controllers 
{
  [Route("api/[controller]")]
  [ApiController]
    [RealEstate.Service.Classes.Authorize]
    public class UploadFileController :Controller
    {
        private readonly IWebHostEnvironment _env;

        public UploadFileController(IWebHostEnvironment env)
        {
            _env = env;


        }
        [HttpPost("FileUpload")]
        public async Task<IActionResult> index()
        {
            var files = Request.Form.Files;
            if (files == null || files.Count == 0)
                return Content("file not selected");
            long size = files.Sum(f => f.Length);
            var filePaths = new List<string>();
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    // full path to file in temp location
                    var folderName = Path.Combine("wwwroot/UploadFiles");
                    if (!Directory.Exists(folderName))
                    {
                        Directory.CreateDirectory(folderName);
                    }

                    string extension = Path.GetExtension(formFile.FileName);

                   
                    string fileName = Path.GetFileNameWithoutExtension(formFile.FileName);
                    var fileNameWithDate = fileName + DateTime.Now.Ticks;
                    var filepath = Path.Combine(folderName, fileNameWithDate + extension);
                  

                    using (var stream = new FileStream(filepath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    } 
                    filePaths.Add(fileNameWithDate + extension);
                }
            }
            // process uploaded files
            // Don't rely on or trust the FileName property without validation.
            return Ok(new { count = files.Count, size, filePaths });
        }
    }
}
