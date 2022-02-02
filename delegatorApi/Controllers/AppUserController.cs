using delegatorApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using delegatorApi.Library.Models;
using delegatorApi.Library.DataAccess;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace delegatorApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AppUserController : ControllerBase
    {
        [HttpGet]
        public AppUser GetById()
        {
            AppUserData data = new();

            return data.GetUserById(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }
    }
}
