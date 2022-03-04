using delegatorApi.Library.Models;
using delegatorApi.Library.Models.Context;
using System.Collections.Generic;
using System.Linq;

namespace delegatorApi.Library.DataAccess
{
    public class CompanyData
    {
        public void Post(Company company)
        {
            using (delegatorContext db = new())
            {
                db.Companies.Add(company);
                db.SaveChanges();
            }
        }

        public void Update(Company company)
        {
            using (delegatorContext db = new())
            {
                db.Companies.Update(company);
                db.SaveChanges();
            }
        }

        public Company GetById(string id)
        {
            using (delegatorContext db = new())
            {
                return db.Companies.FirstOrDefault(c => c.Id== id);
            }
        }

        public List<Company> GetByUserId(string id)
        {
            using (delegatorContext db = new())
            {
                return db.CompanyUsers.Where(cu => cu.AppUserId == id).Select(cu => cu.Company).ToList();
            }
        }

        public Company GetByTitle(string title)
        {
            using (delegatorContext db = new())
            {
                return db.Companies.FirstOrDefault(c => c.Title.ToLower() == title.ToLower());
            }
        }

        public List<Company> GetWhereTitleContains(string s)
        {
            using (delegatorContext db = new())
            {
                return db.Companies.Where(c => c.Title.ToLower().Contains(s.ToLower())).ToList();
            }
        }
    }
}
