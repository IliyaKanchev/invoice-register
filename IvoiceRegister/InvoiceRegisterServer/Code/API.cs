using System;
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
}
