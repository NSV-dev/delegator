using delegatorUI.Library.Api.Helpers.Base;
using delegatorUI.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace delegatorUI.Library.Api.Helpers
{
    public class AppUserHelper : BaseHelper
    {
        private readonly CompaniesUsersHelper _companiesUsersHelper;

        public AppUserHelper(HttpClient apiClient, CompaniesUsersHelper companiesUsersHelper)
            : base(apiClient)
        {
            _companiesUsersHelper = companiesUsersHelper;
        }

        public async Task Post(AppUser user)
        {
            using (HttpResponseMessage resp = await _apiClient.PostAsJsonAsync("AppUser", new User(user)))
            {
                if (resp.IsSuccessStatusCode)
                    return;
                else
                    throw new Exception(resp.ReasonPhrase);
            }
        }

        public async Task<AppUser> GetByUsername(string name)
        {
            using (HttpResponseMessage resp = await _apiClient.GetAsync($"AppUser/ByUsername?name={name}"))
            {
                if (resp.IsSuccessStatusCode)
                {
                    User user = await resp.Content.ReadAsAsync<User>();
                    if (user is null)
                        return null;
                    else
                        return new AppUser(user);
                } else
                    throw new Exception(resp.ReasonPhrase);
            }
        }

        public async Task<List<AppUser>> GetByCompany(string companyID)
        {
            using (HttpResponseMessage resp = await _apiClient.GetAsync($"AppUser/ByCompanyID?companyID={companyID}"))
            {
                if (resp.IsSuccessStatusCode)
                    return await ConvertToAppUsersWithRole(await resp.Content.ReadAsAsync<List<User>>(), companyID);
                else
                    throw new Exception(resp.ReasonPhrase);
            }
        }

        public async Task<List<AppUser>> GetByTask(string taskID)
        {
            using (HttpResponseMessage resp = await _apiClient.GetAsync($"AppUser/ByTask?taskID={taskID}"))
            {
                if (resp.IsSuccessStatusCode)
                    return ConvertToAppUsersWithoutRole(await resp.Content.ReadAsAsync<List<User>>());
                else
                    throw new Exception(resp.ReasonPhrase);
            }
        }

        public async Task<List<AppUser>> GetWhereNameContains(string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return null;
            using (HttpResponseMessage resp = await _apiClient.GetAsync($"AppUser/WhereNameContains?s={s}"))
            {
                if (resp.IsSuccessStatusCode)
                    return ConvertToAppUsersWithoutRole(await resp.Content.ReadAsAsync<List<User>>());
                else
                    throw new Exception(resp.ReasonPhrase);
            }
        }

        public List<AppUser> ConvertToAppUsersWithoutRole(List<User> users)
        {
            List<AppUser> result = new();

            foreach (User user in users)
                result.Add(new AppUser(user));

            return result;
        }

        public async Task<List<AppUser>> ConvertToAppUsersWithRole(List<User> users, string companyID)
        {
            List<AppUser> result = new();

            foreach (User user in users)
                result.Add(new AppUser(user)
                {
                    Role = (await _companiesUsersHelper.GetByCompanyId(companyID, user.Id)).Single().Role
                });

            return result;
        }
    }
}
