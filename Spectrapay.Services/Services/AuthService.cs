using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Spectrapay.Models.DtoModels;
using Spectrapay.Models.Entities;
using Spectrapay.Models.ViewModels;
using Spectrapay.Services.Data;
using Spectrapay.Services.IServices;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Spectrapay.Services.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly DataContext _context;

        public AuthService(IConfiguration configuration, DataContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public async Task<DataResponse<LoginView>> Login(LoginUserDTO request)
        {
            var loginResponse = new DataResponse<LoginView>();

            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);

                if(user == null)
                {
                    loginResponse.Status = false;
                    loginResponse.StatusMessage = "User Not Found";
                    return loginResponse;
                }

                bool isPasswordValid = VerifyPasswordHash(request.Password, user.PasswordHash!, user.PasswordSalt!);

                if(!isPasswordValid )
                {
                    loginResponse.Status = false;
                    loginResponse.StatusMessage = "Incorrect Password, or Username, how can we know?";
                    return loginResponse;
                }

                string token = CreateToken(user);
                user.VerificationToken = token;
                await _context.SaveChangesAsync();

                var loginData = new LoginView
                {
                    Username = request.Username!,
                    VerificationToken = token,
                };

                loginResponse.Status = true;
                loginResponse.StatusMessage = "Successful";
                loginResponse.Data = loginData;
            }
            catch (Exception)
            {
                loginResponse.Status = false;
                loginResponse.StatusMessage = "Unsuccessful, An Error occured!";
                return loginResponse;
            }
            return loginResponse;
        }
        public bool VerifyPasswordHash(string password,
            byte[] passwordHash,
            byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username!)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                            _configuration.GetSection("AppSettings:Token").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }
}
