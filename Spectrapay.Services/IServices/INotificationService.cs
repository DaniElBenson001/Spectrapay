using Spectrapay.Models.DtoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spectrapay.Services.IServices
{
    public interface INotificationService
    {
        //Task<DataResponse<string>> NotifyUser(NotificationDTO notification);
        Task AddNotificationAsync(int userId, string message);
        Task<List<NotificationDTO>> GetAllNotifications();
    }
}
