﻿using delegatorApi.Library.Models;
using delegatorApi.Library.Models.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace delegatorApi.Library.DataAccess
{
    public class CompanyUserData
    {
        public void Post(CompanyUser companyUser)
        {
            using (delegatorContext db = new())
            {
                db.CompanyUsers.Add(companyUser);
                db.SaveChanges();
            }
        }
        
        public void Update(CompanyUser companyUser)
        {
            using (delegatorContext db = new())
            {
                db.CompanyUsers.Update(companyUser);
                db.SaveChanges();
            }
        }

        public void Delete(CompanyUser companyUser)
        {
            using (delegatorContext db = new())
            {
                db.CompanyUsers.Remove(companyUser);
                db.SaveChanges();
            }
        }

        public List<CompanyUser> GetByCompanyAndUserId(string CompanyId, string AppUserId)
        {
            using (delegatorContext db = new())
            {
                return db.CompanyUsers.Where(cu => cu.CompanyId == CompanyId && cu.AppUserId == AppUserId)
                    .Include(cu => cu.Role)
                    .Include(cu => cu.Company)
                    .Include(cu => cu.AppUser)
                    .ToList();
            }
        }
    }
}
