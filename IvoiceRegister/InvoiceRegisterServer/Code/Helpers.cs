using System;
namespace InvoiceRegisterServer.Code
{
    public class AppSettings
    {
        private string _connectionString;
        private string _secret;

        public AppSettings()
        {
            _connectionString = "";
            _secret = "";
        }

        public string Secret { get => _secret; set => _secret = value; }
        public string ConnectionString { get => _connectionString; set => _connectionString = value; }
    }

    public class Authentication
    {
        private string _username;
        private string _password;

        public Authentication()
        {
            _username = "";
            _password = "";
        }

        public string Username { get => _username; set => _username = value; }
        public string Password { get => _password; set => _password = value; }
    }
}
