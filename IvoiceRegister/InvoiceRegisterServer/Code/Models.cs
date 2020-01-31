using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace InvoiceRegisterServer.Code
{
    public class Invoice
    {
        private int _id;
        private int _number;
        private DateTime _date;
        private string _description;
        private double _sum;

        private int _clientId;
        private Client _client;

        public Invoice()
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
        [JsonIgnore] public Client Client { get => _client; set => _client = value; }
    }

    public class Client
    {
        private int _id;
        private string _name;

        private List<Invoice> _invoices;

        public Client()
        {
            _id = 0;
            _name = "";
        }

        public int Id { get => _id; set => _id = value; }
        public string Name { get => _name; set => _name = value; }

        public List<Invoice> Invoices { get => _invoices; set => _invoices = value; }
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // create relations
            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.Client)
                .WithMany(c => c.Invoices)
                .HasForeignKey(i => i.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            // initial seed

            // clients seed
            Client client1 = new Client() { Id = 1, Name = "Client1" };
            Client client2 = new Client() { Id = 2, Name = "Client2" };

            modelBuilder.Entity<Client>().HasData(client1);
            modelBuilder.Entity<Client>().HasData(client2);

            //invoices seed
            Invoice invoice1 = new Invoice { Id = 1, ClientId = client1.Id, Number = 1, Description = "description 1", Sum = 100.0 };
            Invoice invoice2 = new Invoice { Id = 2, ClientId = client1.Id, Number = 2, Description = "description 2", Sum = 200.0 };
            Invoice invoice3 = new Invoice { Id = 3, ClientId = client2.Id, Number = 3, Description = "description 3", Sum = 300.0 };
            Invoice invoice4 = new Invoice { Id = 4, ClientId = client2.Id, Number = 4, Description = "description 4", Sum = 400.0 };

            modelBuilder.Entity<Invoice>().HasData(invoice1);
            modelBuilder.Entity<Invoice>().HasData(invoice2);
            modelBuilder.Entity<Invoice>().HasData(invoice3);
            modelBuilder.Entity<Invoice>().HasData(invoice4);

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //set connection string
            optionsBuilder.UseSqlServer("Data Source=127.0.0.1,1433;Database=test;User Id=SA;Password=TEST_p@ssword1;");

            base.OnConfiguring(optionsBuilder);
        }
    }
}
