using delegatorApi.Library.DataAccess;
using delegatorApi.Library.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace delegatorApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly RoleData _roleData;

        public RoleController(RoleData roleData)
        {
            _roleData = roleData;
        }

        public List<Role> Get() => _roleData.Get();

        [Route("ByTitle")]
        public Role GetByTitle(string title) => _roleData.GetByTitle(title);
    }
}
