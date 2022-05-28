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
        public string Post(Complited complited)
        {
            using (delegatorContext db = new())
            {
                db.Complited.Add(complited);
                db.SaveChanges();
                return complited.Id;
            }
        }

        public List<Complited> GetByTaskCode(string taskCode)
        {
            using (delegatorContext db = new())
            {
                return db.Complited.Where(c => c.TaskCode == taskCode)
                    .Include(c => c.CompanyUser).ThenInclude(cu => cu.AppUser)
                    .ToList();
            }
        }

        public List<Complited> GetByCompanyAndUser(string companyID, string userID)
        {
            using (delegatorContext db = new())
            {
                return db.Complited.Where(c => c.CompanyUser.CompanyId == companyID && c.CompanyUser.AppUserId == userID)
                    .Include(c => c.CompanyUser).ThenInclude(cu => cu.AppUser)
                    .ToList();
            }
        }

        public List<Complited> GetByCompanyAndUserAndDate(string companyID, string userID, DateTime from, DateTime to)
        {
            using (delegatorContext db = new())
            {
                return db.Complited.Where(c => c.CompanyUser.CompanyId == companyID && c.CompanyUser.AppUserId == userID &&
                    c.EndTime >= from && c.EndTime <= to)
                    .ToList();
            }
        }
    }
}
