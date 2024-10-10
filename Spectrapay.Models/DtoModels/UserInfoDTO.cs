using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spectrapay.Models.DtoModels
{
    public record class UserInfoDTO
    {
        public string? Username { get; set; } = string.Empty;
        public Guid? AcctId { get; set; }
        public decimal? VirtBalance { get; set; }

    }
}
