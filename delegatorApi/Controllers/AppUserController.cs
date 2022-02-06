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
    public class AppUserController : ControllerBase
    {
        public List<AppUser> GetAll() => new AppUserData().GetAll();

        [Route("ByComp")]
        public List<AppUser> GetByCompanyId(string compId) => new AppUserData().GetByCompanyId(compId);
    }
}
