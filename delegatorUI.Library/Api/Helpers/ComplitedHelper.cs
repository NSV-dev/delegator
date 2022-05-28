using delegatorUI.Library.Api.Helpers.Base;
using delegatorUI.Library.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace delegatorUI.Library.Api.Helpers
{
    public class ComplitedHelper : BaseHelper
    {
        public ComplitedHelper(HttpClient apiClient)
            : base(apiClient) { }

        public async Task<string> Post(Complited complited)
        {
            using (HttpResponseMessage resp = await _apiClient.PostAsJsonAsync("Complited", complited))
            {
                if (resp.IsSuccessStatusCode)
                    return await resp.Content.ReadAsStringAsync();
                else
                    throw new Exception(resp.ReasonPhrase);
            }
        }

        public async Task<List<Complited>> GetByTaskCode(string taskCode)
        {
            using (HttpResponseMessage resp = await _apiClient.GetAsync($"Complited/ByTaskCode?taskCode={taskCode}"))
            {
                if (resp.IsSuccessStatusCode)
                    return await resp.Content.ReadAsAsync<List<Complited>>();
                else
                    throw new Exception(resp.ReasonPhrase);
            }
        }

        public async Task<List<Complited>> GetByCompanyAndUser(string companyID, string userID)
        {
            using (HttpResponseMessage resp = await _apiClient.GetAsync($"Complited/ByCompanyAndUser?companyID={companyID}&userID={userID}"))
            {
                if (resp.IsSuccessStatusCode)
                {
                    var a = await resp.Content.ReadAsAsync<List<Complited>>();
                    return a;
                } else
                    throw new Exception(resp.ReasonPhrase);
            }
        }

        public async Task<List<Complited>> GetByCompanyAndUserAndDate(string companyId, string userID, DateTime from, DateTime to)
        {
            using (HttpResponseMessage resp = await _apiClient.GetAsync
                ($"Complited/ByCompanyAndUserAndDate?companyID={companyId}&userID={userID}&from={from.ToString("yyyy.MM.dd")}&to={to.ToString("yyyy.MM.dd")}"))
            {
                if (resp.IsSuccessStatusCode)
                {
                    var a = await resp.Content.ReadAsAsync<List<Complited>>();
                    return a;
                } else
                    throw new Exception(resp.ReasonPhrase);
            }
        }
    }
}
