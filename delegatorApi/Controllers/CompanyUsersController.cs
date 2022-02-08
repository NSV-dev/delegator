using delegatorApi.Library.DataAccess;
using delegatorApi.Library.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace delegatorApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyUsersController : ControllerBase
    {
        private readonly CompanyUserData _companyUserData;

        public CompanyUsersController(CompanyUserData companyUserData)
        {
            _companyUserData = companyUserData;
        }

        [Route("ByCompanyAndUserId")]
        public List<CompanyUser> GetByCompanyId(string CompanyId, string AppUserId) => _companyUserData.GetByCompanyAndUserId(CompanyId, AppUserId);
    }
}
