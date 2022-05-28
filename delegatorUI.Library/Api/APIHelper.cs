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
        public TaskHelper Tasks;
        public CategoryHelper Categories;
        public ComplitedHelper Complited;
        public ComplitedFileHelper ComplitedFile;
        public AppFileHelper AppFile;

        private void InitializeHelpers()
        {
            Companies = new(_apiClient);
            CompaniesUsers = new(_apiClient);
            Users = new(_apiClient, CompaniesUsers);
            Roles = new(_apiClient);
            Tasks = new(_apiClient, Users);
            Categories = new(_apiClient);
            Complited = new(_apiClient);
            ComplitedFile = new(_apiClient);
            AppFile = new(_apiClient);
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
