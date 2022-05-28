using delegatorApi.Library.DataAccess;
using delegatorApi.Library.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace delegatorApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly FileData _fileData;

        public FileController(FileData fileData)
        {
            _fileData = fileData;
        }

        public string Post(File file) => _fileData.Post(file);

        [Route("ById")]
        public File GetById(string fileID) => _fileData.GetById(fileID);
    }
}
