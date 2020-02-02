using System;
using System.Collections.Generic;
using System.Net.Http;
using InvoiceRegisterClient.Helpers;
using InvoiceRegisterClient.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace InvoiceRegisterClient.Services
{
    public interface IClientsService
    {
        PagedResultViewModel<ClientViewModel> List(string token);
    }

    public class ClientsService : IClientsService
    {
        private readonly AppSettings _appSettings;

        public ClientsService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public PagedResultViewModel<ClientViewModel> List(string token)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_appSettings.ApiURL);
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                //HTTP POST
                var postTask = client.PostAsJsonAsync("/api/clients/list", new { });
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    string jsonContent = result.Content.ReadAsStringAsync().Result;
                    PagedResultViewModel<ClientViewModel> contact = JsonConvert.DeserializeObject<PagedResultViewModel<ClientViewModel>>(jsonContent);

                    return contact;
                }
            }

            return null;
        }
    }
}
