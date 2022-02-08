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
        [Route("ByCompanyAndUserId")]
        public List<CompanyUser> GetByCompanyId(string CompanyId, string AppUserId) => new CompanyUserData().GetByCompanyAndUserId(CompanyId, AppUserId);
    }
}
