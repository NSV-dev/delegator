using delegatorApi.Library.Models;
using delegatorApi.Library.Models.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace delegatorApi.Library.DataAccess
{
    public class ComplitedData
    {
        public void Post(Complited complited)
        {
            using (delegatorContext db = new())
            {
                db.Complited.Add(complited);
                db.SaveChanges();
            }
        }

        public List<Complited> GetByTask(string taskID)
        {
            using (delegatorContext db = new())
            {
                return db.Complited.Where(c => c.TaskId == taskID).ToList();
            }
        }

            public List<Complited> GetByUserAndDate(string userID, DateTime from, DateTime to)
        {
            using (delegatorContext db = new())
            {
                return db.Complited.Where(c => c.UserId == userID &&
                    c.EndTime >= from && c.EndTime <= to)
                    .Include(c => c.Task)
                    .ToList();
            }
        }
    }
}
