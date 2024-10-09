using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spectrapay.Models.ViewModels
{
    public record class LoginView
    {
        public string Username { get; set; } = string.Empty;
        public string? VerificationToken { get; set; }
    }
}
