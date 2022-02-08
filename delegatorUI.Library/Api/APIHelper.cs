using delegatorUI.Library.Api.Helpers;
using delegatorUI.Library.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace delegatorUI.Library.Api
{
    public class APIHelper
    {
        private HttpClient _apiClient;

        #region Helpers
        public AppUserHelper Users;
        public CompaniesHelper Companies;
        public CompaniesUsersHelper CompaniesUsers;

        private void InitializeHelpers()
        {
            Users = new(_apiClient);
            Companies = new(_apiClient);
            CompaniesUsers = new(_apiClient);
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
