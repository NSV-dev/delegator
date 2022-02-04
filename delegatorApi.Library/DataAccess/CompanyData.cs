using delegatorApi.Library.Models;
using delegatorApi.Library.Models.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace delegatorApi.Library.DataAccess
{
    public class CompanyData
    {
        public List<Company> GetAll()
        {
            using delegatorContext db = new();
            return db.Companies.ToList();
        }

        public List<Company> GetByAdminId(string id)
        {
            using (delegatorContext db = new())
            {
                var cus = db.CompanyUsers.Where(cu => cu.Role.Title == "Admin" && cu.AppUserId == id).ToList();
                List<Company> res = new();
                foreach (var c in db.Companies.ToList())
                    foreach (var cu in cus)
                        if (c.Id == cu.CompanyId)
                            res.Add(c);
                return res;
            }
        }
    }
}
