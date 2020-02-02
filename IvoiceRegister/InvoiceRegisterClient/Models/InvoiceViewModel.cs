using System;
using Newtonsoft.Json;

namespace InvoiceRegisterClient.Models
{
    public class InvoiceViewModel
    {
        private int _id;
        private int _number;
        private DateTime _date;
        private string _description;
        private double _sum;

        private int _clientId;

        public InvoiceViewModel()
        {
            _id = 0;
            _number = 0;
            _date = DateTime.Now;
            _description = "";
            _sum = 0.0;
        }

        public int Id { get => _id; set => _id = value; }
        public int Number { get => _number; set => _number = value; }
        public DateTime Date { get => _date; set => _date = value; }
        public string Description { get => _description; set => _description = value; }
        public double Sum { get => _sum; set => _sum = value; }

        [JsonProperty("client_id")] public int ClientId { get => _clientId; set => _clientId = value; }
    }
}
