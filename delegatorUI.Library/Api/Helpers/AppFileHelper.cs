using delegatorUI.Library.Api.Helpers.Base;
using delegatorUI.Library.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace delegatorUI.Library.Api.Helpers
{
    public class AppFileHelper : BaseHelper
    {
        public AppFileHelper(HttpClient apiClient) : base(apiClient)
        { }

        public async Task<string> Post(AppFile file)
        {
            using (HttpResponseMessage resp = await _apiClient.PostAsJsonAsync("File", file))
            {
                if (resp.IsSuccessStatusCode)
                    return await resp.Content.ReadAsStringAsync();
                else
                    throw new Exception(resp.ReasonPhrase);
            }
        }

        public async Task<AppFile> GetById(string fileID)
        {
            using (HttpResponseMessage resp = await _apiClient.GetAsync($"File/ById?fileID={fileID}"))
            {
                if (resp.IsSuccessStatusCode)
                    return await resp.Content.ReadAsAsync<AppFile>();
                else
                    throw new Exception(resp.ReasonPhrase);
            }
        }
    }
}
