using delegatorApi.Library.Models;
using delegatorApi.Library.Models.Context;
using System.Collections.Generic;
using System.Linq;

namespace delegatorApi.Library.DataAccess
{
    public class TaskData
    {
        public void Post(Task task)
        {
            using (delegatorContext db = new())
            {
                db.Tasks.Add(task);
                db.SaveChanges();
            }
        }

        public List<Task> GetByUserIDAndCompanyID(string userID, string companyID)
        {
            using (delegatorContext db = new())
            {
                return db.TasksUsers
                    .Where(tu => tu.UserId == userID && tu.CompanyId == companyID)
                    .Select(tu => tu.Task)
                    .ToList();
            }
        }

        public List<Task> GetByTaskID(string taskID)
        {
            using (delegatorContext db = new())
            {
                return db.TasksTasks
                    .Where(tt => tt.MainTaskId == taskID)
                    .Select(tt => tt.Task)
                    .ToList();
            }
        }
    }
}
