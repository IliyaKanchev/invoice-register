using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace InvoiceRegisterServer.Code
{
    public class ApiError
    {
        private readonly string _error;

        public ApiError(string err)
        {
            _error = err;
        }

        public string Error { get => _error; }
    }

    public class PagedResult<T> where T : class
    {
        private readonly int _page;
        private readonly int _pagesSize;
        private readonly int _pagesCount;

        private readonly List<T> _items;

        public PagedResult(int page, int pagesSize, int pagesCount, List<T> items)
        {
            _page = page;
            _pagesSize = pagesSize;
            _pagesCount = pagesCount;
            _items = items;
        }

        public int Page { get => _page; }
        [JsonProperty("page_size")] public int PageSize { get => _pagesSize; }
        [JsonProperty("pages_count")] public int PagesCount { get => _pagesCount; }

        public List<T> Items { get => _items; }
    }
}
