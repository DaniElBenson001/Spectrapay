using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spectrapay.Models.Entities
{
    public record class User
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public byte[]? PasswordHash { get; set; } = new byte[32];
        public byte[]? PasswordSalt { get; set; } = new byte[32];
        public decimal? VirtualBalance { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsLocked { get; set;}
        public bool? IsVerified { get; set; }
        public Guid AccountId {  get; set; }
        public string VerificationToken { get; set; } = string.Empty;
    }
}
