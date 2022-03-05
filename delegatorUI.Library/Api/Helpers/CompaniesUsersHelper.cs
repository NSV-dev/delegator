using delegatorUI.Library.Api.Helpers.Base;
using delegatorUI.Library.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace delegatorUI.Library.Api.Helpers
{
    public class CompaniesUsersHelper : BaseHelper
    {
        public CompaniesUsersHelper(HttpClient apiClient)
            : base(apiClient) { }

        public async Task Post(CompanyUser companyUser)
        {
            using (HttpResponseMessage resp = await _apiClient.PostAsJsonAsync("CompanyUsers", companyUser))
            {
                if (resp.IsSuccessStatusCode)
                    return;
                else
                    throw new Exception(resp.ReasonPhrase);
            }
        }

        public async Task Update(CompanyUser companyUser)
        {
            using (HttpResponseMessage resp = await _apiClient.PostAsJsonAsync("CompanyUsers/Update", companyUser))
            {
                if (resp.IsSuccessStatusCode)
                    return;
                else
                    throw new Exception(resp.ReasonPhrase);
            }
        }

        public async Task Delete(CompanyUser companyUser)
        {
            using (HttpResponseMessage resp = await _apiClient.PostAsJsonAsync("CompanyUsers/Delete", companyUser))
            {
                if (resp.IsSuccessStatusCode)
                    return;
                else
                    throw new Exception(resp.ReasonPhrase);
            }
        }

        public async Task<List<CompanyUser>> GetByCompanyId(string CompanyId, string AppUserId)
        {
            using (HttpResponseMessage resp = await _apiClient.GetAsync($"CompanyUsers/ByCompanyAndUserId?CompanyId={CompanyId}&AppUserId={AppUserId}"))
            {
                if (resp.IsSuccessStatusCode)
                {
                    return await resp.Content.ReadAsAsync<List<CompanyUser>>();
                } else
                    throw new Exception(resp.ReasonPhrase);
            }
        }
    }
}
