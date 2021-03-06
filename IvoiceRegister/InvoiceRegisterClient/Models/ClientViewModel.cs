﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

    // visualization helper, containing private List<InvoiceViewModel> filtering data
    public class ClientViewModelWithInvoices : ClientViewModel
    {
        private DateTime _switch;

        private int _invoiceId;
        private int _invoiceNumber;
        private DateTime _invoiceDate;
        private string _invoiceDescription;
        private double _invoiceSum;
        private DateTime _invoiceBefore;
        private DateTime _invoiceAfter;
        private bool _invoiceReversed;
        private int _invoicePage;
        private int _invoicePageSize;
        private int _invoicePagesCount;

        public ClientViewModelWithInvoices()
        {
            _switch = DateTime.FromBinary(0);
            //_switch = new DateTime(_switch.Year, _switch.Month, _switch.Day, _switch.Hour, _switch.Minute, _switch.Second);


            _invoiceId = 0;
            _invoiceNumber = 0;
            _invoiceDate = _switch;
            _invoiceDescription = "";
            _invoiceSum = 0.0;
            _invoiceAfter = _switch;
            _invoiceBefore = _switch;
            _invoiceReversed = false;
            _invoicePage = 1;
            _invoicePageSize = 0;
            _invoicePagesCount = 0;

        }

        [JsonIgnore] public int InvoiceId { get => _invoiceId; set => _invoiceId = value; }
        [JsonIgnore] public int InvoiceNumber { get => _invoiceNumber; set => _invoiceNumber = value; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = true)]
        [JsonIgnore] public DateTime InvoiceDate { get => _invoiceDate; set => _invoiceDate = value; }

        [JsonIgnore] public string InvoiceDescription { get => _invoiceDescription; set => _invoiceDescription = value; }
        [JsonIgnore] public double InvoiceSum { get => _invoiceSum; set => _invoiceSum = value; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = true)]
        [JsonIgnore] public DateTime InvoiceBefore { get => _invoiceBefore; set => _invoiceBefore = value; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = true)]
        [JsonIgnore] public DateTime InvoiceAfter { get => _invoiceAfter; set => _invoiceAfter = value; }

        [JsonIgnore] public bool InvoiceReversed { get => _invoiceReversed; set => _invoiceReversed = value; }
        [JsonIgnore] public int InvoicePage { get => _invoicePage; set => _invoicePage = value; }
        [JsonIgnore] public int InvoicePageSize { get => _invoicePageSize; set => _invoicePageSize = value; }
        [JsonIgnore] public int InvoicePagesCount { get => _invoicePagesCount; set => _invoicePagesCount = value; }

        // a factory to ease search parameters creation
        public JObject ToInvoicesSearch()
        {
            JObject search = new JObject
            {
                { "reversed", _invoiceReversed }
            };

            if (_invoiceId != 0) search.Add("id", _invoiceId);
            if (Id != 0) search.Add("client_id", Id);
            if (_invoiceNumber != 0) search.Add("number", _invoiceNumber);
            if (_invoiceDate != _switch) search.Add("date", _invoiceDate);
            if (!string.IsNullOrEmpty(_invoiceDescription)) search.Add("description", _invoiceDescription);
            if (!_invoiceSum.Equals(0.0)) search.Add("sum", _invoiceSum);
            if (_invoiceBefore != _switch) search.Add("before", _invoiceBefore);
            if (_invoiceAfter != _switch) search.Add("after", _invoiceAfter);
            if (_invoicePage > 0) search.Add("page", _invoicePage);
            if (_invoicePageSize > 0) search.Add("page_size", _invoicePageSize);

            //Console.WriteLine(search);
            //Console.WriteLine(_switch);


            return search;
        }

        public void UpdateFromInvoicesPagedResult(PagedResultViewModel<InvoiceViewModel> pagedResult)
        {
            Invoices.Clear();
            Invoices.AddRange(pagedResult.Items);

            _invoicePage = pagedResult.Page;
            _invoicePageSize = pagedResult.PageSize;
            _invoicePagesCount = pagedResult.PagesCount;
        }

        public string ToSerializeInvoiceSearch()
        {
            string data = JsonConvert.SerializeObject(ToInvoicesSearch());
            byte[] buffer = Encoding.UTF8.GetBytes(data);
            string serialized = Convert.ToBase64String(buffer);

            return serialized;
        }

        public void UpdateFromSearchSerialization(string b64)
        {
            byte[] buffer = Convert.FromBase64String(b64);
            string data = Encoding.UTF8.GetString(buffer);

            //Console.WriteLine(data);

            JObject search = JsonConvert.DeserializeObject<JObject>(data);

            //Console.WriteLine(search);

            foreach (JProperty property in search.Properties())
            {
                if (property.Name == "id") _invoiceId = search.SelectToken(property.Name).Value<int>();
                if (property.Name == "client_id") Id = search.SelectToken(property.Name).Value<int>();
                if (property.Name == "number") _invoiceNumber = search.SelectToken(property.Name).Value<int>();
                if (property.Name == "date") _invoiceDate = search.SelectToken(property.Name).Value<DateTime>();
                if (property.Name == "description") _invoiceDescription = search.SelectToken(property.Name).Value<string>();
                if (property.Name == "sum") _invoiceSum = search.SelectToken(property.Name).Value<double>();
                if (property.Name == "before") _invoiceBefore = search.SelectToken(property.Name).Value<DateTime>();
                if (property.Name == "after") _invoiceAfter = search.SelectToken(property.Name).Value<DateTime>();
                if (property.Name == "page") _invoicePage = search.SelectToken(property.Name).Value<int>();
                if (property.Name == "page_size") _invoicePageSize = search.SelectToken(property.Name).Value<int>();
                if (property.Name == "reversed") _invoiceReversed = search.SelectToken(property.Name).Value<bool>();
            }
        }
    }
}
