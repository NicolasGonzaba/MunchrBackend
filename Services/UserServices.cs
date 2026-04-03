using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MunchrBackend.Context;
using MunchrBackend.Models;
using MunchrBackend.Models.DTOS;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MunchrBackend.Services;

    public class UserServices
    {
         private readonly DataContext _dataContext;
        private readonly IConfiguration _config;

        public UserServices(DataContext dataContext, IConfiguration config)
        {
            _dataContext = dataContext;
            _config = config;
        }


        public async Task<bool> CreateAccount(UserDTO newUser)
        {
            if(await DoesUserExist(newUser.Username)) return false;

            UserModel user = new();
            PasswordDTO EncryptedPassword = HashPassword(newUser.Password);
            user.Username = newUser.Username;
            user.Email= newUser.Email;
            user.Buissness= newUser.Buissness;
            user.Password=newUser.Password;
            user.Hash = EncryptedPassword.Hash;
            user.Salt = EncryptedPassword.Salt;

            await _dataContext.Users.AddAsync(user);
            return await _dataContext.SaveChangesAsync() != 0;
        }
        private async Task<bool> DoesUserExist(string username)
        {
            return await _dataContext.Users.SingleOrDefaultAsync(user => user.Username == username) != null;
        }

        private static PasswordDTO HashPassword(string password)
        {
            byte[] SaltBytes = RandomNumberGenerator.GetBytes(64);

            string salt = Convert.ToBase64String(SaltBytes);

            string hash;

            using (var derivedBytes = new Rfc2898DeriveBytes(password, SaltBytes, 310000, HashAlgorithmName.SHA256))
            {
                hash = Convert.ToBase64String(derivedBytes.GetBytes(32));
            }

            return new PasswordDTO
            {
                Salt = salt,
                Hash = hash
            };
        }

        public async Task<string> Login (LogInDTO user)
        {
            UserModel currentUser = await GetUserInfoByUsernameAsync(user.Username);

            if(currentUser == null) return null;

            if(!VerifyPassword(user.Password, currentUser.Salt, currentUser.Hash)) return null;

            return GenerateJWT(new List<Claim>());
        }

        private string GenerateJWT(List<Claim> claims)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]));
            var SigningCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokenOptions = new JwtSecurityToken(
                issuer: "",
                audience: "",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: SigningCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        private static bool VerifyPassword(string password, string salt, string hash)
        {
            byte[] saltByte = Convert.FromBase64String(salt);

            string checkHash;

            using(var derivedBytes = new Rfc2898DeriveBytes(password, saltByte, 310000, HashAlgorithmName.SHA256))
            {
                checkHash = Convert.ToBase64String(derivedBytes.GetBytes(32));
                return hash == checkHash;
            }
        }



        public async Task<UserModel> GetUserInfoByUsernameAsync(string username) => await _dataContext.Users.SingleOrDefaultAsync(user => user.Username == username);
        
        public async Task<UserInfoDTO> GetUserByUsername(string username)
        {
            var currentUser = await _dataContext.Users.SingleOrDefaultAsync(user => user.Username ==username);

            UserInfoDTO user = new();
            user.Id = currentUser.Id;
            user.Username = currentUser.Username;
            return user;
        }
        public async Task<bool> DeleteAccount(UserModel userToDelete){
            _dataContext.Users.Remove(userToDelete);
            return await _dataContext.SaveChangesAsync() !=0;
        }
    }
