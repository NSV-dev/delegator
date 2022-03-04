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

        public void Post(AppUser appUser) => _appUserData.Post(appUser);

        [Route("Update")]
        public void Update(AppUser appUser) => _appUserData.Update(appUser);

        [Route("ByUsername")]
        public AppUser GetByUesrname(string name) => _appUserData.GetByUsername(name);

        [Route("ByCompanyID")]
        public List<AppUser> GetByCompanyId(string companyID) => _appUserData.GetByCompanyId(companyID);

        [Route("ByTask")]
        public List<AppUser> GetByTaskId(string taskID) => _appUserData.GetByTask(taskID);

        [Route("WhereNameContains")]
        public List<AppUser> GetWhereTitleContains(string s) => _appUserData.GetWhereNameContains(s);
    }
}
