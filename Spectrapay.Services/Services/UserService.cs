using Microsoft.AspNetCore.Http;
using Spectrapay.Models.DtoModels;
using Microsoft.EntityFrameworkCore;
using Spectrapay.Services.Data;
using Spectrapay.Models.Entities;
using Spectrapay.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Spectrapay.Services.Services
{
    public class UserService : IUserService
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<DataResponse<string>> CreateUser(CreateUserDTO request)
        {
            DataResponse<string> dataResponse = new();
            var username = await _context.Users.AnyAsync(u => u.Username == request.Username);

            PasswordHashGenerator(request.Password!,
                out byte[] passwordHash,
                out byte[] passwordSalt);

            try
            {
                var userData = new User
                {
                    Username = request.Username,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    AccountId = Guid.NewGuid(),
                    CreatedAt = DateTime.Now,
                    IsDeleted = false,
                    IsLocked = false,
                    IsVerified = false,
                    VirtualBalance = 10000,
                };

                if (username)
                {
                    dataResponse.Status = false;
                    dataResponse.StatusMessage = "Unsuccessful, User Already Exists";
                    return dataResponse;
                }

                await _context.Users.AddAsync(userData);
                await _context.SaveChangesAsync();


                dataResponse.Status = true;
                dataResponse.StatusMessage = "Successful";
                return dataResponse;
            }
            catch(Exception)
            {
                dataResponse.Status = false;
                dataResponse.StatusMessage = "Unsuccessful, an Error occured!";
                return dataResponse;
            }
        }

        public async Task<DataResponse<UserInfoDTO>> GetUserInfo()
        {
            DataResponse<UserInfoDTO> infoResponse = new();

            int userID;

            try
            {
                userID = Convert.ToInt32(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier));

                if(_httpContextAccessor.HttpContext == null)
                {
                    infoResponse.Status = false;
                    infoResponse.StatusMessage = "User Not Found!";
                    return infoResponse;
                }

                var userData = await _context.Users
                   .Where(u => u.Id == userID)
                   .Select(u => new UserInfoDTO
                   {
                       Username = u.Username,
                       AcctId = u.AccountId,
                       VirtBalance = u.VirtualBalance
                   })
                   .FirstOrDefaultAsync();

                infoResponse.Status = true;
                infoResponse.StatusMessage = "Successful";
                infoResponse.Data = userData;
                return infoResponse;
            }
            catch( Exception )
            {
                infoResponse.Status = false;
                infoResponse.StatusMessage = "Error Occurred Somewhere!";
                return infoResponse;
            }
        }

        public void PasswordHashGenerator(string password,
            out byte[] passwordHash,
            out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public bool PasswordHashVerifier(string password,
            byte[] passwordHash,
            byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}
