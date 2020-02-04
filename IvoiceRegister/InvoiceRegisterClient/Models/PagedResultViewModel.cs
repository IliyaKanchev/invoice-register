using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace InvoiceRegisterClient.Models
{
    public class PagedResultViewModel<T> where T : class
    {
        private int _page;
        private int _pagesSize;
        private int _pagesCount;

        private List<T> _items;

        public PagedResultViewModel()
        { 
            _page = 1;
            _pagesSize = 0;
            _pagesCount = 0;

            _items = new List<T>();
        }

        public int Page { get => _page; set => _page = value; }
        [JsonProperty("page_size")] public int PageSize { get => _pagesSize; set => _pagesSize = value; }
        [JsonProperty("pages_count")] public int PagesCount { get => _pagesCount; set => _pagesCount = value; }

        public List<T> Items { get => _items; set => _items = value; }
    }

    // visualization helper, containing private List<ClientViewModel> filtering data
    public class PagedClientsViewModel : PagedResultViewModel<ClientViewModelWithInvoices>
    {
        private string _name;
        private int _id;

        public PagedClientsViewModel()
        {
            _id = 0;
            _name = "";
        }

        public int Id { get => _id; set => _id = value; }
        public string Name { get => _name; set => _name = value; }
    }
}
