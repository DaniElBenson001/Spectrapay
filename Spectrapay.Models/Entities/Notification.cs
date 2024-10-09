using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spectrapay.Models.Entities
{
    public record class Notification
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string? Message { get; set; }
        public bool IsRead { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
