using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spectrapay.Models.DtoModels
{
    public record class PaymentDTO
    {
        public string SenderAcctId {  get; set; } = string.Empty;
        public string ReceivcerAccId { get; set; } = string.Empty;
        public decimal Amount {  get; set; } 
    }
}
