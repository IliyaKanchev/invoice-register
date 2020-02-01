using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace InvoiceRegisterServer.Code
{
    public class AppSettings
    {
        public string Secret { get; set; }
    }

    public class Authentication
    {
        private string _username;
        private string _password;

        public Authentication()
        {
            _username = "";
            _password = "";
        }

        public string Username { get => _username; set => _username = value; }
        public string Password { get => _password; set => _password = value; }
    }

    public interface IUserService
    {
        User Authenticate(string username, string password);
        IEnumerable<User> GetAll();
    }

    public class UserService : IUserService
    {
        private readonly AppSettings _appSettings;
        private readonly DbServiceContext _context;

        public UserService(IOptions<AppSettings> appSettings, DbServiceContext context)
        {
            _appSettings = appSettings.Value;
            _context = context;
        }

        public User Authenticate(string username, string password)
        {
            User user = null;
            IQueryable<User> users = _context.Users.Where(x => x.Username == username);

            foreach(User item in users)
            {
                byte[] salt = Convert.FromBase64String(item.Salt);

                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

                if (hashed == item.Password)
                {
                    user = item;
                    break;
                }
            }

            // return null if user not found
            if (user == null)
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            // remove password before returning
            user.Password = null;

            return user;
        }

        public IEnumerable<User> GetAll()
        {
            // return users without passwords
            return _context.Users;
        }
    }
}
