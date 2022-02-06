using delegatorApi.Library.DataAccess;
using delegatorApi.Library.Models;
using delegatorApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

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
