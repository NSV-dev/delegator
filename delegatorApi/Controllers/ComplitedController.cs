using delegatorApi.Library.DataAccess;
using delegatorApi.Library.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace delegatorApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComplitedController : ControllerBase
    {
        private readonly ComplitedData _complitedData;

        public ComplitedController(ComplitedData complitedData)
        {
            _complitedData = complitedData;
        }

        public void Post(Complited complited) => _complitedData.Post(complited);

        [Route("ByTask")]
        public List<Complited> GetByTask(string taskID)
            => _complitedData.GetByTask(taskID);

        [Route("ByUserAndDate")]
        public List<Complited> GetByUserAndDate(string userID, DateTime from, DateTime to)
            => _complitedData.GetByUserAndDate(userID, from, to);
    }
}
