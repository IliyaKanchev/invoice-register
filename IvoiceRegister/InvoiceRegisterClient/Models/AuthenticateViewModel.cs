using System;
using System.ComponentModel.DataAnnotations;

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
        [DataType(DataType.Password)] public string Password { get => _password; set => _password = value; }
    }
}
