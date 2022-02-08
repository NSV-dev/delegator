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
        private readonly AppUserData _appUserData;

        public AppUserController(AppUserData appUserData)
        {
            _appUserData = appUserData;
        }

        public List<AppUser> GetAll() => new AppUserData().GetAll();

        [Route("ByUsername")]
        public AppUser GetByUesrname(string name) => _appUserData.GetByUsername(name);

        [Route("ByComp")]
        public List<AppUser> GetByCompanyId(string compId) => _appUserData.GetByCompanyId(compId);
    }
}
