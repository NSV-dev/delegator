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

        public async Task<List<Complited>> GetByUserAndDate(string userID, DateTime from, DateTime to)
        {
            using (HttpResponseMessage resp = await _apiClient.GetAsync($"Complited/ByUserAndDate?userID={userID}&from={from.ToString("yyyy.MM.dd")}&to={to.ToString("yyyy.MM.dd")}"))
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
