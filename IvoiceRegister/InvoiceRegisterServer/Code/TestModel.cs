using System;
namespace InvoiceRegisterServer.Code
{
    public class TestModel
    {
        private string _text;

        public TestModel()
        {
            _text = "";
        }

        public string Text
        {
            get {
                return _text;
            } set {
                _text = value;
            }
        }
    }
}
