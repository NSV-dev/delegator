﻿using delegatorApi.Library.DataAccess;
using delegatorApi.Library.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace delegatorApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly TaskData _taskData;

        public TaskController(TaskData taskData)
        {
            _taskData = taskData;
        }

        public void Post(Task task) => _taskData.Post(task);

        [Route("ByUserIDAndCompanyID")]
        public List<Task> GetByUserIDAndCompanyID(string userID, string companyID) => _taskData.GetByUserIDAndCompanyID(userID, companyID);

        [Route("ByTaskID")]
        public List<Task> GebByTaskID(string taskID) => _taskData.GetByTaskID(taskID);
    }
}
