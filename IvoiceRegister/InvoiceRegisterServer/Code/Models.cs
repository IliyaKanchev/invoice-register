using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace InvoiceRegisterServer.Code
{
    public class User
    {
        private int _id;
        private string _username;
        private string _password;
        private string _salt;
        private string _token;

        public User()
        {
            _id = 0;
            _username = "";
            _password = "";
            _salt = "";
            _token = "";
        }

        public int Id { get => _id; set => _id = value; }
        public string Username { get => _username; set => _username = value; }
        [JsonIgnore] public string Password { get => _password; set => _password = value; }
        [JsonIgnore] public string Salt { get => _salt; set => _salt = value; }
        [NotMapped] public string Token { get => _token; set => _token = value; }       
    }

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
        private DbSet<User> _users;

        private readonly bool _hasConnectionString;

        public DbServiceContext()
        {
            _hasConnectionString = false;
        }

        public DbServiceContext(DbContextOptions<DbServiceContext> options) : base(options)
        {
            _hasConnectionString = true;
        }

        public DbSet<Client> Clients { get => _clients; set => _clients = value; }
        public DbSet<Invoice> Invoices { get => _invoices; set => _invoices = value; }
        public DbSet<User> Users { get => _users; set => _users = value; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // create relations
            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.Client)
                .WithMany(c => c.Invoices)
                .HasForeignKey(i => i.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            // initial seed

            // users seed
            // generate a 128-bit salt using a secure PRNG
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: "root",
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            modelBuilder.Entity<User>().HasData(new User { Id = 1, Username = "root", Password = hashed, Salt = Convert.ToBase64String(salt) });

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
            if (!_hasConnectionString) optionsBuilder.UseSqlServer("Data Source=127.0.0.1,1433;Database=test;User Id=SA;Password=TEST_p@ssword1;");

            base.OnConfiguring(optionsBuilder);
        }
    }
}
