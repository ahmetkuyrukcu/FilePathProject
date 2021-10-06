using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FilePathProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private IResult _result;

        public FilesController(IResult result)
        {
            _result = result;
        }

        [HttpGet("list")]
        public IActionResult List([FromBody] File model)
        {

            if (model.Path != null)
            {
                try
                {
                    var files = Directory.GetFiles(model.Path);
                    var directories = Directory.GetDirectories(model.Path);

                    foreach (var directory in directories)
                    {
                        DirectoryInfo directoryInfo = new DirectoryInfo(directory);
                        _result.DirectoryResult.Add(directoryInfo.Name);
                    }

                    foreach (var file in files)
                    {
                        FileInfo fileInfo = new FileInfo(file);
                        _result.FileResult.Add(fileInfo.Name);
                    }

                    return Ok(_result);
                }
                catch (Exception exception)
                {
                    return BadRequest(exception.Message); ;
                }
            }

            return BadRequest();
        }
    }

    public class File
    {
        public string Path { get; set; }
    }

    public class Result : IResult
    {
        public Result()
        {
            DirectoryResult = new List<string>();
            FileResult = new List<string>();
        }
        public List<string> DirectoryResult { get; set; }
        public List<string> FileResult { get; set; }
    }

    public interface IResult
    {
        public List<string> DirectoryResult { get; set; }
        public List<string> FileResult { get; set; }
    }
}
