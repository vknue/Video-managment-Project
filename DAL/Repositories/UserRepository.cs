using AutoMapper;
using AutoMapper.Internal.Mappers;
using DAL.BLModels;
using DAL.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public interface IUserRepository
    {
        IEnumerable<BLUser> GetAll();
        BLUser GetById(int id);
        void ConfirmEmail(string email, string SecurityToken);
        void ChangePassword(string username, string newPassword);
        BLUser CreateUser(string username, string firstName, string lastName, string email, string password, string countryOfResidence, string PhoneNumber);
        BLUser GetConfirmedUser(string username, string password);
        void DeactivateUser(int id);
    }

    public class UserRepository : IUserRepository
    {
        private readonly RwaMoviesContext _db;
        private readonly IMapper _mapper;

        public UserRepository(RwaMoviesContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public IEnumerable<BLUser> GetAll()
            => (IEnumerable<BLUser>)_mapper.Map<IEnumerable<BLUser>>(_db.Users);

        public BLUser GetById(int id)
        =>  _mapper.Map<BLUser>(_db.Users.FirstOrDefault(x => x.Id == id, null));
        

        public void ChangePassword(string username, string newPassword)
        {
            var user = _db.Users.FirstOrDefault(x =>
            x.Username == username && !x.DeletedAt.HasValue);

            (var salt, var b64Salt) = GenerateSalt();

            var b64Hash = CreateHash(newPassword, salt);

            user.PwdHash = b64Hash;
            user.PwdSalt = b64Salt;

            _db.SaveChanges();
        }

        public void ConfirmEmail(string email, string SecurityToken)
        {
            var userToConfirm = _db.Users.FirstOrDefault(x =>
                x.Email == email && x.SecurityToken == SecurityToken);

            userToConfirm.IsConfirmed = true;
            _db.SaveChanges();
        }
        
        public void DeactivateUser(int id)
        {
            var target = _db.Users.FirstOrDefault(x => x.Id == id);
            target.DeletedAt = DateTime.UtcNow;
            _db.SaveChanges();
        }
        

        public BLUser CreateUser(string username, string firstName, string lastName, string email, string password, string countryOfResidence, string phone)
        {
            username = username.ToLower().Trim();
            if (_db.Users.Any(x => x.Username.Equals(username)))
                throw new InvalidOperationException("Username already exists");

            (var salt, var b64salt) = GenerateSalt();
            var b64Hash = CreateHash(password, salt);
            var b64SecToken = GenerateSecurityToken();

            User userToInsert = new()
            {
                CreatedAt = DateTime.UtcNow,
                Username = username,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                PwdHash = b64Hash,
                PwdSalt = b64salt,
                SecurityToken = b64SecToken,
                CountryOfResidenceId = _db.Countries.First(x => x.Name == countryOfResidence).Id,
                IsConfirmed = false,
                Phone = phone

            };

            _db.Users.Add(userToInsert);
            _db.SaveChanges();

            return _mapper.Map<BLUser>(userToInsert);
        }
        public BLUser GetConfirmedUser(string username, string password)
        {
            User user = _db.Users.First(x =>
            x.Username == username && x.IsConfirmed);
            if (user is null) return null;
            var salt = Convert.FromBase64String(user.PwdSalt);
            var b64Hash = CreateHash(password, salt);

            if (user.PwdHash != b64Hash) throw new InvalidOperationException("Wrong username or password");

            return _mapper.Map<BLUser>(user);
        }

        private static (byte[], string) GenerateSalt()
        {
            var salt = RandomNumberGenerator.GetBytes(128/8);
            var b64salt = Convert.ToBase64String(salt);

            return (salt, b64salt);
        }

        private static string CreateHash(string password, byte[] salt)
        {
            byte[] hash =
                KeyDerivation.Pbkdf2(
                    password: password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 100000,
                    numBytesRequested: 256 / 8);
            string b64Hash = Convert.ToBase64String(hash);

            return b64Hash;
        }
        
        private static string GenerateSecurityToken()
        {
            byte[] securityToken = RandomNumberGenerator.GetBytes(256 / 8);
            string b64SecToken = Convert.ToBase64String(securityToken);

            return b64SecToken;
        }

       
    }
}
