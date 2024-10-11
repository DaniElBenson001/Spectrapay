using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spectrapay.Models.DtoModels
{
    public record class LoginTokenDTO
    {
        public string Username { get; set; } = string.Empty;
        public string? VerificationToken { get; set; }
    }
}
