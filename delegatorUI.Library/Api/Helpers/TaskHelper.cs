using delegatorUI.Library.Api.Helpers.Base;
using delegatorUI.Library.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace delegatorUI.Library.Api.Helpers
{
    public class TaskHelper : BaseHelper
    {
        private readonly AppUserHelper _appUserHelper;

        public TaskHelper(HttpClient apiClient, AppUserHelper appUserHelper)
            : base(apiClient)
        {
            _appUserHelper = appUserHelper;
        }

        public async Task<List<AppTask>> GetByCompany(string companyID)
        {
            using (HttpResponseMessage resp = await _apiClient.GetAsync($"Task/ByCompanyID?companyID={companyID}"))
            {
                if (resp.IsSuccessStatusCode)
                {
                    List<AppTask> tasks = await resp.Content.ReadAsAsync<List<AppTask>>();

                    tasks = await RemoveNotMains(tasks);

                    foreach (var task in tasks)
                    {
                        task.Users = await _appUserHelper.GetByTask(task.Id);
                        task.Tasks = await GetByTaskID(task.Id);
                    }

                    return tasks;
                } else
                    throw new Exception(resp.ReasonPhrase);
            }
        }

        public async Task<List<AppTask>> GetByUserAndCompany(string userID, string companyID)
        {
            using (HttpResponseMessage resp = await _apiClient.GetAsync($"Task/ByUserIDAndCompanyID?userID={userID}&companyID={companyID}"))
            {
                if (resp.IsSuccessStatusCode)
                {
                    List<AppTask> tasks = await resp.Content.ReadAsAsync<List<AppTask>>();

                    tasks = await RemoveNotMains(tasks);

                    foreach (var task in tasks)
                    {
                        task.Users = await _appUserHelper.GetByTask(task.Id);
                        task.Tasks = await GetByTaskID(task.Id);
                    }

                    return tasks;
                } else
                    throw new Exception(resp.ReasonPhrase);
            }
        }

        public async Task<List<AppTask>> GetByTaskID(string taskID)
        {
            using (HttpResponseMessage resp = await _apiClient.GetAsync($"Task/ByTaskID?taskID={taskID}"))
            {
                if (resp.IsSuccessStatusCode)
                {
                    var tasks = await resp.Content.ReadAsAsync<List<AppTask>>();
                    foreach (var task in tasks)
                    {
                        task.Users = await _appUserHelper.GetByTask(task.Id);
                        task.Tasks = await GetByTaskID(task.Id);
                    }
                    return tasks;
                } else
                    throw new Exception(resp.ReasonPhrase);
            }
        }

        private async Task<List<AppTask>> RemoveNotMains(List<AppTask> tasks)
        {
            List<AppTask> tasksToRemove = new();
            foreach (AppTask task in tasks)
                if (!await IsMain(task.Id))
                    tasksToRemove.Add(task);

            foreach (var task in tasksToRemove)
                tasks.Remove(task);

            return tasks;
        }

        private async Task<bool> IsMain(string taskID)
        {
            using (HttpResponseMessage resp = await _apiClient.GetAsync($"Task/IsMain?taskID={taskID}"))
            {
                if (resp.IsSuccessStatusCode)
                    return await resp.Content.ReadAsAsync<bool>();
                else
                    throw new Exception(resp.ReasonPhrase);
            }
        }
    }
}
