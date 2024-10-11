using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spectrapay.Models.DtoModels
{
    public record class NotificationMessageDTO
    {
        public string SenderMessage { get; set; } = string.Empty;
        public string ReceiverMessage {  get; set; } = string.Empty;
    }
}
