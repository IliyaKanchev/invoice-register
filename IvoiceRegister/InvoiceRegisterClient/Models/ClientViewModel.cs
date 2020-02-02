using System;
using System.Collections.Generic;

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
        }

        public int Id { get => _id; set => _id = value; }
        public string Name { get => _name; set => _name = value; }

        public List<InvoiceViewModel> Invoices { get => _invoices; set => _invoices = value; }
    }
}
