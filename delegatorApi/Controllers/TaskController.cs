using delegatorApi.Library.DataAccess;
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

        [Route("Delete")]
        public void Delete(Task task) => _taskData.Delete(task);

        [Route("Update")]
        public void Update(Task task) => _taskData.Update(task);

        [Route("ByCompanyID")]
        public List<Task> GetCompanyID(string companyID) => _taskData.GetByCompanyID(companyID);

        [Route("ByUserIDAndCompanyID")]
        public List<Task> GetByUserIDAndCompanyID(string userID, string companyID) => _taskData.GetByUserIDAndCompanyID(userID, companyID);

        [Route("ByTaskID")]
        public List<Task> GebByTaskID(string taskID) => _taskData.GetByTaskID(taskID);

        [Route("IsMain")]
        public bool IsMain(string taskID) => _taskData.IsMain(taskID);
    }
}
