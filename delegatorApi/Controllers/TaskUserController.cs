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
    }
}
