using System;
namespace InvoiceRegisterClient.Models
{
    public class AuthenticateViewModel
    {
        private string _username;
        private string _password;

        public AuthenticateViewModel()
        {
            _username = "";
            _password = "";
        }

        public string Username { get => _username; set => _username = value; }
        public string Password { get => _password; set => _password = value; }
    }
}
