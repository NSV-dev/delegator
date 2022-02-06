using delegatorApi.Library.Models;
using delegatorApi.Library.Models.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace delegatorApi.Library.DataAccess
{
    public class CompanyUserData
    {
        public List<CompanyUser> GetByCompanyAndUserId(string CompanyId, string AppUserId)
        {
            using (delegatorContext db = new())
            {
                return db.CompanyUsers.Where(cu => cu.CompanyId == CompanyId && cu.AppUserId == AppUserId)
                    .Include(cu => cu.Role).ToList();
            }
        }
    }
}
