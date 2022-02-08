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

        [Route("ByUserId")]
        public List<Company> GetByUserId(string id) => _companyData.GetByUserId(id);
    }
}
