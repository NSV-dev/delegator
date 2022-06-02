using delegatorApi.Library.DataAccess;
using delegatorApi.Library.Models;
using Microsoft.AspNetCore.Mvc;

namespace delegatorApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskUserController : ControllerBase
    {
        private readonly TaskUserData _taskUserData;

        public TaskUserController(TaskUserData taskUserData)
        {
            _taskUserData = taskUserData;
        }

        public void Post(TasksUser tasksUser) => _taskUserData.Post(tasksUser);

        [Route("Delete")]
        public void Delete(TasksUser tasksUser) => _taskUserData.Delete(tasksUser);

        [Route("Update")]
        public void Update(TasksUser tasksUser) => _taskUserData.Update(tasksUser);

        [Route("Get")]
        public TasksUser Get(TasksUser tasksUser) => _taskUserData.Get(tasksUser);

        [Route("ByTaskAndUser")]
        public TasksUser GetByTaskAndUser(string taskID, string userID) 
            => _taskUserData.GetByTaskAndUser(taskID, userID);
    }
}
