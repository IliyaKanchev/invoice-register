using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace InvoiceRegisterClient.Models
{
    public class ClientViewModel
    {
        private int _id;
        private string _name;

        private List<InvoiceViewModel> _invoices;

        public ClientViewModel()
        {
            _id = 0;
            _name = "";

            _invoices = new List<InvoiceViewModel>();
        }

        [JsonProperty("id")] public int Id { get => _id; set => _id = value; }
        [JsonProperty("name")] public string Name { get => _name; set => _name = value; }

        [JsonProperty("invoices")] public List<InvoiceViewModel> Invoices { get => _invoices; set => _invoices = value; }
    }
}
