﻿using delegatorUI.Library.Api.Helpers.Base;
using delegatorUI.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace delegatorUI.Library.Api.Helpers
{
    public class TaskHelper : BaseHelper
    {
        public TaskHelper(HttpClient apiClient) 
            : base(apiClient) {}

        public async Task<List<AppTask>> GetByUserAndCompany(string userID, string companyID)
        {
            using (HttpResponseMessage resp = await _apiClient.GetAsync($"Task/ByUserIDAndCompanyID?userID={userID}&companyID={companyID}"))
            {
                if (resp.IsSuccessStatusCode)
                {
                    List<AppTask> tasks = await resp.Content.ReadAsAsync<List<AppTask>>();

                    List<AppTask> tasksToRemove = new();
                    foreach (AppTask task in tasks)
                        if (!await IsMain(task.Id))
                            tasksToRemove.Add(task);

                    foreach (var task in tasksToRemove)
                        tasks.Remove(task);

                    foreach (AppTask task in tasks)
                        task.Tasks = await GetByTaskID(task.Id);
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
                        task.Tasks = await GetByTaskID(task.Id);
                    return tasks;
                } else
                    throw new Exception(resp.ReasonPhrase);
            }
        }

        public async Task<bool> IsMain(string taskID)
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
