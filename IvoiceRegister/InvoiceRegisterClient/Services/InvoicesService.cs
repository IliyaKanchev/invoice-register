using System;
using System.Net.Http;
using InvoiceRegisterClient.Helpers;
using InvoiceRegisterClient.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace InvoiceRegisterClient.Services
{
    public interface IInvoicesService
    {
        bool List(string token, ClientViewModelWithInvoices model);
        bool Save(string token, InvoiceViewModel model);
        bool Add(string token, InvoiceViewModel model);
        int Delete(string token, int id);
    }

    public class InvoicesService : IInvoicesService
    {
        private readonly AppSettings _appSettings;

        public InvoicesService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public bool List(string token, ClientViewModelWithInvoices model)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_appSettings.ApiURL);
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                //HTTP POST
                var postTask = client.PostAsJsonAsync("/api/invoices/list", model.ToInvoicesSearch());
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    string jsonContent = result.Content.ReadAsStringAsync().Result;
                    PagedResultViewModel<InvoiceViewModel> response = JsonConvert.DeserializeObject<PagedResultViewModel<InvoiceViewModel>>(jsonContent);
                    
                    model.UpdateFromInvoicesPagedResult(response);

                    return true;
                }
            }

            return false;
        }

        public bool Save(string token, InvoiceViewModel model)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_appSettings.ApiURL);
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                //HTTP POST
                var postTask = client.PostAsJsonAsync("/api/invoices/update", model);
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

        public bool Add(string token, InvoiceViewModel model)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_appSettings.ApiURL);
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                //HTTP POST
                var postTask = client.PostAsJsonAsync("/api/invoices/insert", model);
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

        public int Delete(string token, int id)
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
                var postTask = client.PostAsJsonAsync("/api/invoices/delete", search);
                postTask.Wait();

                var result = postTask.Result;
                string jsonContent = result.Content.ReadAsStringAsync().Result;
                JObject response = JsonConvert.DeserializeObject<JObject>(jsonContent);

                if (result.IsSuccessStatusCode)
                {
                    return response.SelectToken("client_id").Value<int>();
                }

                Console.WriteLine("# error: " + response.SelectToken("error").Value<string>());
            }

            return 0;
        }
    }
}
