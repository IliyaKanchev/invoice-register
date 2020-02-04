using System;
using System.Collections.Generic;
using System.Net.Http;
using InvoiceRegisterClient.Helpers;
using InvoiceRegisterClient.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace InvoiceRegisterClient.Services
{
    public interface IClientsService
    {
        bool List(string token, PagedClientsViewModel model);
        bool Save(string token, ClientViewModelWithInvoices model);
        bool Add(string token, ClientViewModelWithInvoices model);
        bool Delete(string token, int id);
    }

    public class ClientsService : IClientsService
    {
        private readonly AppSettings _appSettings;

        public ClientsService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public bool List(string token, PagedClientsViewModel model)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_appSettings.ApiURL);
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                JObject search = new JObject();

                if (model.Id > 0) search.Add("id", model.Id);
                if (!string.IsNullOrEmpty(model.Name)) search.Add("name", model.Name);
                if (model.PageSize != 0) search.Add("page_size", model.PageSize);
                search.Add("page", model.Page);

                //HTTP POST
                var postTask = client.PostAsJsonAsync("/api/clients/list", search);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    string jsonContent = result.Content.ReadAsStringAsync().Result;
                    PagedClientsViewModel response = JsonConvert.DeserializeObject<PagedClientsViewModel>(jsonContent);

                    model.Items.Clear();
                    model.Items.AddRange(response.Items);
                    model.Page = response.Page;
                    model.PagesCount = response.PagesCount;
                    model.PageSize = response.PageSize;
                    model.Id = response.Id;
                    model.Name = response.Name;

                    return true;
                }
            }

            return false;
        }

        public bool Save(string token, ClientViewModelWithInvoices model)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_appSettings.ApiURL);
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                //HTTP POST
                var postTask = client.PostAsJsonAsync("/api/clients/update", model);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return true;
                }

                string jsonContent = result.Content.ReadAsStringAsync().Result;
                JObject response = JsonConvert.DeserializeObject<JObject>(jsonContent);

                Console.WriteLine("# error: " + response.SelectToken("error").Value<string>());
            }

            return false;
        }

        public bool Add(string token, ClientViewModelWithInvoices model)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_appSettings.ApiURL);
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                //HTTP POST
                var postTask = client.PostAsJsonAsync("/api/clients/insert", model);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return true;
                }

                string jsonContent = result.Content.ReadAsStringAsync().Result;
                JObject response = JsonConvert.DeserializeObject<JObject>(jsonContent);

                Console.WriteLine("# error: " + response.SelectToken("error").Value<string>());
            }

            return false;
        }

        public bool Delete(string token, int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_appSettings.ApiURL);
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                JObject search = new JObject
                {
                    { "id", id }
                };

                //HTTP POST
                var postTask = client.PostAsJsonAsync("/api/clients/delete", search);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return true;
                }

                string jsonContent = result.Content.ReadAsStringAsync().Result;
                JObject response = JsonConvert.DeserializeObject<JObject>(jsonContent);

                Console.WriteLine("# error: " + response.SelectToken("error").Value<string>());
            }

            return false;
        }
    }
}
