using delegatorUI.Library.Api.Helpers;
using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;

namespace delegatorUI.Library.Api
{
    public class APIHelper
    {
        private HttpClient _apiClient;

        #region Helpers
        public AppUserHelper Users;
        public CompaniesHelper Companies;
        public CompaniesUsersHelper CompaniesUsers;
        public RoleHelper Roles;

        private void InitializeHelpers()
        {
            Users = new(_apiClient);
            Companies = new(_apiClient);
            CompaniesUsers = new(_apiClient);
            Roles = new(_apiClient);
        }
        #endregion

        public APIHelper()
        {
            InitializeClient();
            InitializeHelpers();
        }

        private void InitializeClient()
        {
            _apiClient = new();
            _apiClient.BaseAddress = new Uri(ConfigurationManager.AppSettings["api"]);
            _apiClient.DefaultRequestHeaders.Accept.Clear();
            _apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
