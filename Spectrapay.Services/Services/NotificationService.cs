using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Spectrapay.Models.DtoModels;
using Spectrapay.Models.Entities;
using Spectrapay.Services.Data;
using Spectrapay.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Spectrapay.Services.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataContext _context;

        public NotificationService(IHttpContextAccessor httpContextAccessor, DataContext context)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task AddNotificationAsync(int userId, string message)
        {
            Notification newNotification = new Notification
            {
                Message = message,
                IsRead = false,
                CreatedAt = DateTime.Now,
                UserId = userId
            };

            await _context.Notifications.AddAsync(newNotification);
            await _context.SaveChangesAsync();
        }

        public async Task<List<NotificationDTO>> GetAllNotifications()
        {
            List<NotificationDTO> getNotif = new();
            int userId;

            try
            {
                userId = Convert.ToInt32(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier));
                var userNotifData = await _context.Notifications.Where(n => n.UserId == userId).ToListAsync();

                foreach(var notification in userNotifData)
                {
                    getNotif.Add(new NotificationDTO()
                    {
                        Message = notification.Message,
                        IsRead = notification.IsRead,
                        CreatedAt = notification.CreatedAt,
                        UserId = userId
                    });

                }
                return getNotif;
            }
            catch(Exception)
            {
                return new List<NotificationDTO>();
            }
        }
    }
}
