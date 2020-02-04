using System;
using System.Collections.Generic;
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
            _invoiceId = 0;
            _invoiceNumber = 0;
            _invoiceDate = DateTime.FromFileTimeUtc(0);
            _invoiceDescription = "";
            _invoiceSum = 0.0;
            _invoiceAfter = DateTime.FromFileTimeUtc(0);
            _invoiceBefore = DateTime.FromFileTimeUtc(0);
            _invoiceReversed = false;
            _invoicePage = 0;
            _invoicePageSize = 0;
            _invoicePagesCount = 0;

        }

        [JsonIgnore] public int InvoiceId { get => _invoiceId; set => _invoiceId = value; }
        [JsonIgnore] public int InvoiceNumber { get => _invoiceNumber; set => _invoiceNumber = value; }
        [JsonIgnore] public DateTime InvoiceDate { get => _invoiceDate; set => _invoiceDate = value; }
        [JsonIgnore] public string InvoiceDescription { get => _invoiceDescription; set => _invoiceDescription = value; }
        [JsonIgnore] public double InvoiceSum { get => _invoiceSum; set => _invoiceSum = value; }
        [JsonIgnore] public DateTime InvoiceBefore { get => _invoiceBefore; set => _invoiceBefore = value; }
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
                { "client_id", Id },
                { "reversed", _invoiceReversed }
            };

            if (_invoiceId != 0) search.Add("id", _invoiceId);
            if (_invoiceNumber != 0) search.Add("number", _invoiceNumber);
            if (_invoiceDate != DateTime.FromFileTimeUtc(0)) search.Add("date", _invoiceDate);
            if (!string.IsNullOrEmpty(_invoiceDescription)) search.Add("description", _invoiceDescription);
            if (!_invoiceSum.Equals(0.0)) search.Add("sum", _invoiceSum);
            if (_invoiceBefore != DateTime.FromFileTimeUtc(0)) search.Add("before", _invoiceBefore);
            if (_invoiceAfter != DateTime.FromFileTimeUtc(0)) search.Add("after", _invoiceAfter);
            if (_invoicePage > 0) search.Add("page", _invoicePage);
            if (_invoicePageSize > 0) search.Add("page_size", _invoicePageSize);

            return search;
        }

        public void FillFromInvoicesPagedResult(PagedResultViewModel<InvoiceViewModel> pagedResult)
        {
            Invoices.Clear();
            Invoices.AddRange(pagedResult.Items);

            _invoicePage = pagedResult.Page;
            _invoicePageSize = pagedResult.PageSize;
            _invoicePagesCount = pagedResult.PagesCount;
        }
    }
}
