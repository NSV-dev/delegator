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

        public string Post(Complited complited) => _complitedData.Post(complited);

        [Route("ByTaskCode")]
        public List<Complited> GetByTaskCode(string taskCode)
            => _complitedData.GetByTaskCode(taskCode);

        [Route("ByCompanyAndUser")]
        public List<Complited> GetByCompanyUser(string companyID, string userID)
            => _complitedData.GetByCompanyAndUser(companyID, userID);

        [Route("ByCompanyAndUserAndDate")]
        public List<Complited> GetByCompanyUserAndDate(string companyID, string userID, DateTime from, DateTime to)
            => _complitedData.GetByCompanyAndUserAndDate(companyID, userID, from, to);
    }
}
