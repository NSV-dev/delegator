using delegatorApi.Library.DataAccess;
using delegatorApi.Library.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace delegatorApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppUserController : ControllerBase
    {
        public List<AppUser> GetAll() => new AppUserData().GetAll();

        [Route("ByUsername")]
        public AppUser GetByUesrname(string name) => new AppUserData().GetByUsername(name);

        [Route("ByComp")]
        public List<AppUser> GetByCompanyId(string compId) => new AppUserData().GetByCompanyId(compId);
    }
}
