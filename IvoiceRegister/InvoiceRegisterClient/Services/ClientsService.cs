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
        bool List(string token, PagedResultViewModel<ClientViewModel> model);
    }

    public class ClientsService : IClientsService
    {
        private readonly AppSettings _appSettings;

        public ClientsService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public bool List(string token, PagedResultViewModel<ClientViewModel> model)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_appSettings.ApiURL);
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                JObject search = new JObject();

                if (model.Id > 0) search.Add("id", model.Id);
                if (!string.IsNullOrEmpty(model.Name)) search.Add("name", model.Name);
                if (model.Page != 0) search.Add("page", model.Page);
                if (model.PageSize != 0) search.Add("page_size", model.PageSize);

                //HTTP POST
                var postTask = client.PostAsJsonAsync("/api/clients/list", search);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    string jsonContent = result.Content.ReadAsStringAsync().Result;
                    PagedResultViewModel<ClientViewModel> contact = JsonConvert.DeserializeObject<PagedResultViewModel<ClientViewModel>>(jsonContent);

                    model.Items.Clear();
                    model.Items.AddRange(contact.Items);

                    return true;
                }
            }

            return false;
        }
    }
}
