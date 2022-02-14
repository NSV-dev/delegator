using delegatorApi.Library.DataAccess;
using delegatorApi.Library.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace delegatorApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskTaskController : ControllerBase
    {
        private readonly TaskTaskData _taskTaskData;

        public TaskTaskController(TaskTaskData taskTaskData)
        {
            _taskTaskData = taskTaskData;
        }

        public void Post(TasksTask tasksTask) => _taskTaskData.Post(tasksTask);
    }
}
