using System;
using System.Net.Http;
using InvoiceRegisterClient.Helpers;
using InvoiceRegisterClient.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace InvoiceRegisterClient.Services
{
    public interface IInvoicesService
    {
        bool List(string token, ClientViewModelWithInvoices model);
        bool Save(string token, InvoiceViewModel model);
        bool Add(string token, InvoiceViewModel model);
        bool Delete(string token, int id);
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
            return false;
        }

        public bool Add(string token, InvoiceViewModel model)
        {
            return false;
        }

        public bool Delete(string token, int id)
        {
            return false;
        }
    }
}
