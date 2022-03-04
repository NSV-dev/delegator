using delegatorApi.Library.DataAccess;
using delegatorApi.Library.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace delegatorApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly CompanyData _companyData;

        public CompanyController(CompanyData companyData)
        {
            _companyData = companyData;
        }

        public void Post(Company company) => _companyData.Post(company);

        [Route("Update")]
        public void Update(Company company) => _companyData.Update(company);

        [Route("ById")]
        public Company GetById(string id) => _companyData.GetById(id);

        [Route("ByUserId")]
        public List<Company> GetByUserId(string id) => _companyData.GetByUserId(id);

        [Route("ByTitle")]
        public Company GetByTitle(string title) => _companyData.GetByTitle(title);

        [Route("WhereTitleContains")]
        public List<Company> GetWhereTitleContains(string s) => _companyData.GetWhereTitleContains(s);
    }
}
