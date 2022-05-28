using delegatorApi.Library.DataAccess;
using delegatorApi.Library.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace delegatorApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComplitedFileController : ControllerBase
    {
        private readonly ComplitedFileData _complitedFileData;

        public ComplitedFileController(ComplitedFileData complitedFileData)
        {
            _complitedFileData = complitedFileData;
        }

        public void Post(ComplitedFile complitedFile) => _complitedFileData.Post(complitedFile);

        [Route("ByComplited")]
        public List<ComplitedFile> GetByComplited(string complitedID) => _complitedFileData.GetByComplited(complitedID);
    }
}
