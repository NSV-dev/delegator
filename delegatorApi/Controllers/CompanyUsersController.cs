﻿using delegatorApi.Library.DataAccess;
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

        public void Post(CompanyUser companyUser) => _companyUserData.Post(companyUser);
        
        [Route("Update")]
        public void Update(CompanyUser companyUser) => _companyUserData.Update(companyUser);

        [Route("Delete")]
        public void Delete(CompanyUser companyUser) => _companyUserData.Delete(companyUser);

        [Route("ByCompanyAndUserId")]
        public List<CompanyUser> GetByCompanyId(string CompanyId, string AppUserId) => _companyUserData.GetByCompanyAndUserId(CompanyId, AppUserId);
    }
}
