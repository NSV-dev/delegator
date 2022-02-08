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
        [Route("ByUserId")]
        public List<Company> GetByUserId(string id) => new CompanyData().GetByUserId(id);
    }
}
