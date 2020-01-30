using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace InvoiceRegisterServer.Code
{
    public class Invoice
    {
        private int _id;
        private int _number;
        private DateTime _date;
        private string _description;
        private float _sum;

        private int _clientId;
        private Client _client;


        public Invoice()
        {
        }

        public int Id { get => _id; set => _id = value; }
        public int Number { get => _number; set => _number = value; }
        public DateTime Date { get => _date; set => _date = value; }
        public string Description { get => _description; set => _description = value; }
        public float Sum { get => _sum; set => _sum = value; }
        public int ClientId { get => _clientId; set => _clientId = value; }
        public Client Client { get => _client; set => _client = value; }
    }

    public class Client
    {
        private int _id;
        private string _name;

        private List<Invoice> _invoices;

        public Client()
        {
        }

        public int Id { get => _id; set => _id = value; }
        public string Name { get => _name; set => _name = value; }

        public ICollection<Invoice> Invoices { get => _invoices; set => _invoices = (List<Invoice>)value; }
    }

    public class DbServiceContext : DbContext
    {
        private DbSet<Client> _clients;
        private DbSet<Invoice> _invoices;

        public DbServiceContext()
        {

        }

        public DbSet<Client> Clients { get => _clients; set => _clients = value; }
        public DbSet<Invoice> Invoices { get => _invoices; set => _invoices = value; }
    }
}
