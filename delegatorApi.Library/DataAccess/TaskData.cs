using delegatorApi.Library.Models;
using delegatorApi.Library.Models.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace delegatorApi.Library.DataAccess
{
    public class TaskData
    {
        public void Post(Task task)
        {
            task.CategoryId = task.Category.Id;
            task.SenderId = task.Sender.Id;
            task.ResponsibleId = task.Responsible.Id;
            task.Category = null;
            task.Sender = null;
            task.Responsible = null;
            using (delegatorContext db = new())
            {
                db.Tasks.Add(task);
                db.SaveChanges();
            }
        }

        public void Delete(Task task)
        {
            using (delegatorContext db = new())
            {
                db.Tasks.Remove(task);
                db.SaveChanges();
            }
        }

        public void Update(Task task)
        {
            using (delegatorContext db = new())
            {
                db.Tasks.Update(task);
                db.SaveChanges();
            }
        }

        public List<Task> GetByCompanyID(string companyID)
        {
            using (delegatorContext db = new())
            {
                return db.TasksUsers
                    .Where(tu => tu.CompanyId == companyID)
                    .Select(tu => tu.Task)
                    .Distinct()
                    .Include(t => t.Sender)
                    .Include(t => t.Category)
                    .Include(t => t.Responsible)
                    .ToList();
            }
        }

        public List<Task> GetByUserIDAndCompanyID(string userID, string companyID)
        {
            using (delegatorContext db = new())
            {
                return db.TasksUsers
                    .Where(tu => tu.UserId == userID && tu.CompanyId == companyID)
                    .Select(tu => tu.Task)
                    .Distinct()
                    .Include(t => t.Sender)
                    .Include(t => t.Category)
                    .Include(t => t.Responsible)
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
                    .Distinct()
                    .Include(t => t.Sender)
                    .Include(t => t.Category)
                    .Include(t => t.Responsible)
                    .ToList();
            }
        }

        public bool IsMain(string taskID)
        {
            using (delegatorContext db = new())
            {
                if (db.TasksTasks.Where(tt => tt.TaskId == taskID).Count() == 0)
                    return true;
                else
                    return false;
            }
        }
    }
}
