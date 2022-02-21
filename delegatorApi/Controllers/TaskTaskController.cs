using delegatorApi.Library.DataAccess;
using delegatorApi.Library.Models;
using Microsoft.AspNetCore.Mvc;

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

        [Route("Delete")]
        public void Delete(TasksTask tasksTask) => _taskTaskData.Delete(tasksTask);

        [Route("ByTaskID")]
        public TasksTask GetByTaskId(string taskID) => _taskTaskData.GetByTaskID(taskID);
    }
}
