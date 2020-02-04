using System;
using InvoiceRegisterClient.Helpers;
using InvoiceRegisterClient.Models;
using Microsoft.Extensions.Options;

namespace InvoiceRegisterClient.Services
{
    public interface IInvoicesService
    {
        bool List(string token, PagedResultViewModel<InvoiceViewModel> model);
        bool Save(string token, InvoiceViewModel model);
        bool Add(string token, InvoiceViewModel model);
        bool Delete(string token, int id);
    }

    public class InvoicesService : IInvoicesService
    {
        private readonly AppSettings _appSettings;

        public InvoicesService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public bool Add(string token, InvoiceViewModel model)
        {
            throw new NotImplementedException();
        }

        public bool Delete(string token, int id)
        {
            throw new NotImplementedException();
        }

        public bool List(string token, PagedResultViewModel<InvoiceViewModel> model)
        {
            throw new NotImplementedException();
        }

        public bool Save(string token, InvoiceViewModel model)
        {
            throw new NotImplementedException();
        }
    }
}
