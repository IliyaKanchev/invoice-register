using System;
namespace InvoiceRegisterClient.Helpers
{
    public class AppSettings
    {
        private string _apiURL;

        public AppSettings()
        {
            _apiURL = "http://localhost:47959/api";
        }

        public string ApiURL { get => _apiURL; set => _apiURL = value; }
    }
}
