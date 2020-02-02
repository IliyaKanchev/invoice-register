using System;
namespace InvoiceRegisterClient.Models
{
    public class User
    {
        private int _id;
        private string _username;
        private string _token;

        public User()
        {
            _id = 0;
            _username = "";
            _token = "";
        }

        public int Id { get => _id; set => _id = value; }
        public string Username { get => _username; set => _username = value; }
        public string Token { get => _token; set => _token = value; }
    }
}
