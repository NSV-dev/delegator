using delegatorUI.Library.Api.Helpers.Base;
using delegatorUI.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task Post(AppTask task, string companyID, string mainTaskID = null)
        {
            using (HttpResponseMessage resp = await _apiClient.PostAsJsonAsync("Task", task))
            {
                if (!resp.IsSuccessStatusCode)
                    throw new Exception(resp.ReasonPhrase);
            }

            foreach (AppUser user in task.Users)
            {
                TaskUsers taskUser = new()
                {
                    TaskId = task.Id,
                    UserId = user.Id,
                    CompanyId = companyID
                };
                using (HttpResponseMessage resp = await _apiClient.PostAsJsonAsync("TaskUser", taskUser))
                {
                    if (!resp.IsSuccessStatusCode)
                        throw new Exception(resp.ReasonPhrase);
                }
            }

            if (mainTaskID != null)
            {
                TaskTasks taskTasks = new()
                {
                    MainTaskId = mainTaskID,
                    TaskId = task.Id
                };
                using (HttpResponseMessage resp = await _apiClient.PostAsJsonAsync("TaskTask", taskTasks))
                {
                    if (!resp.IsSuccessStatusCode)
                        throw new Exception(resp.ReasonPhrase);
                }
            }
        }

        public async Task Delete(AppTask task, string companyID)
        {
            if (task.Tasks is not null)
            {
                foreach (AppTask subtask in task.Tasks)
                    Delete(subtask, companyID);

                foreach (AppTask subtask in task.Tasks)
                {
                    TaskTasks taskTasksWithID;
                    using (HttpResponseMessage resp = await _apiClient.GetAsync($"TaskTask/ByTaskID?taskID={subtask.Id}"))
                    {
                        if (resp.IsSuccessStatusCode)
                            taskTasksWithID = await resp.Content.ReadAsAsync<TaskTasks>();
                        else
                            throw new Exception(resp.ReasonPhrase);
                    }
                    using (HttpResponseMessage resp = await _apiClient.PostAsJsonAsync("TaskTask/Delete", taskTasksWithID))
                    {
                        if (!resp.IsSuccessStatusCode)
                            throw new Exception(resp.ReasonPhrase);
                    }
                }
            }
            foreach (AppUser user in task.Users)
            {
                TaskUsers taskUsers = new()
                {
                    CompanyId = companyID,
                    TaskId = task.Id,
                    UserId = user.Id
                };
                TaskUsers taskUsersWithID;
                using (HttpResponseMessage resp = await _apiClient.PostAsJsonAsync("TaskUser/Get", taskUsers))
                {
                    if (resp.IsSuccessStatusCode)
                        taskUsersWithID = await resp.Content.ReadAsAsync<TaskUsers>();
                    else
                        throw new Exception(resp.ReasonPhrase);
                }
                if (taskUsersWithID is not null)
                    using (HttpResponseMessage resp = await _apiClient.PostAsJsonAsync("TaskUser/Delete", taskUsersWithID))
                    {
                        if (!resp.IsSuccessStatusCode)
                            throw new Exception(resp.ReasonPhrase);
                    }
            }

            TaskTasks taskTasks = null;
            using (HttpResponseMessage resp = await _apiClient.GetAsync($"TaskTask/ByTaskID?taskID={task.Id}"))
            {
                if (resp.IsSuccessStatusCode)
                    taskTasks = await resp.Content.ReadAsAsync<TaskTasks>();
                else
                    throw new Exception(resp.ReasonPhrase);
            }
            if (taskTasks != null)
            {
                using (HttpResponseMessage resp = await _apiClient.PostAsJsonAsync("TaskTask/Delete", taskTasks))
                {
                    if (!resp.IsSuccessStatusCode)
                        throw new Exception(resp.ReasonPhrase);
                }
            }

            using (HttpResponseMessage resp = await _apiClient.PostAsJsonAsync("Task/Delete", task))
            {
                if (!resp.IsSuccessStatusCode)
                    throw new Exception(resp.ReasonPhrase);
            }
        }

        public async Task Update(AppTask oldTask, AppTask newTask, string companyID)
        {
            newTask.Id = oldTask.Id;
            using (HttpResponseMessage resp = await _apiClient.PostAsJsonAsync("Task/Update", newTask))
            {
                if (!resp.IsSuccessStatusCode)
                    throw new Exception(resp.ReasonPhrase);
            }

            foreach (AppUser user in newTask.Users)
            {
                if (!oldTask.Users.Contains(user))
                {
                    using (HttpResponseMessage resp = await _apiClient.PostAsJsonAsync("TaskUser", new TaskUsers()
                    {
                        CompanyId = companyID,
                        TaskId = oldTask.Id,
                        UserId = user.Id
                    }))
                    {
                        if (!resp.IsSuccessStatusCode)
                            throw new Exception(resp.ReasonPhrase);
                    }
                }
            }

            foreach (AppUser user in oldTask.Users)
            {
                if (!newTask.Users.Contains(user))
                {
                    TaskUsers taskUsers = new()
                    {
                        CompanyId = companyID,
                        TaskId = oldTask.Id,
                        UserId = user.Id
                    };
                    TaskUsers taskUsersWithID;
                    using (HttpResponseMessage resp = await _apiClient.PostAsJsonAsync("TaskUser/Get", taskUsers))
                    {
                        if (resp.IsSuccessStatusCode)
                            taskUsersWithID = await resp.Content.ReadAsAsync<TaskUsers>();
                        else
                            throw new Exception(resp.ReasonPhrase);
                    }
                    using (HttpResponseMessage resp = await _apiClient.PostAsJsonAsync("TaskUser/Delete", taskUsersWithID))
                    {
                        if (!resp.IsSuccessStatusCode)
                            throw new Exception(resp.ReasonPhrase);
                    }
                }
            }
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

        public async Task<List<AppTask>> GetByUserAndCompanyWithoutSubs(string userID, string companyID)
        {
            using (HttpResponseMessage resp = await _apiClient.GetAsync($"Task/ByUserIDAndCompanyID?userID={userID}&companyID={companyID}"))
            {
                if (resp.IsSuccessStatusCode)
                {
                    var tasks = await resp.Content.ReadAsAsync<List<AppTask>>();

                    foreach (AppTask item in tasks)
                        item.Users = await _appUserHelper.GetByTask(item.Id);

                    return tasks;
                } else
                    throw new Exception(resp.ReasonPhrase);
            }
        }

        public async Task<List<AppTask>> GetByUserAndCompanyOnlyMain(string userID, string companyID)
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
                        task.Tasks = (await GetByTaskID(task.Id)).Where(t => t.Users.Any(u => u.Id == userID)).ToList();
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
