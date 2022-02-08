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

        public async Task<List<CompanyUser>> GetByCompanyId(string CompanyId, string AppUserId)
        {
            using (HttpResponseMessage resp = await _apiClient.GetAsync($"CompanyUsers/ByCompanyAndUserId?CompanyId={CompanyId}&AppUserId={AppUserId}"))
            {
                if (resp.IsSuccessStatusCode)
                    return await resp.Content.ReadAsAsync<List<CompanyUser>>();
                else
                    throw new Exception(resp.ReasonPhrase);
            }
        }
    }
}
