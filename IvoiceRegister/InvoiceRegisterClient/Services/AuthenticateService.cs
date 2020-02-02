using System;
using System.Net.Http;
using InvoiceRegisterClient.Helpers;
using InvoiceRegisterClient.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace InvoiceRegisterClient.Services
{
    public interface IAuthenticateService
    {
        User Authenticate(AuthenticateViewModel authenticate);
    }

    public class AuthenticateService : IAuthenticateService
    {
        private readonly AppSettings _appSettings;

        public AuthenticateService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public User Authenticate(AuthenticateViewModel authenticate)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_appSettings.ApiURL);

                //HTTP POST
                var postTask = client.PostAsJsonAsync("/api/users/authenticate", authenticate);
                postTask.Wait();

                var result = postTask.Result;
                System.Diagnostics.Debug.WriteLine("# status: " + result.StatusCode.ToString());
                if (result.IsSuccessStatusCode)
                {
                    string jsonContent = result.Content.ReadAsStringAsync().Result;
                    User contact = JsonConvert.DeserializeObject<User>(jsonContent);

                    return contact;
                }

                return null;
            }
        }
    }
}
