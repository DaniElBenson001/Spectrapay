using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spectrapay.Models.Entities
{
    public enum Status
    {
        Successful,
        Pending,
        Failed
    }
    public enum TransactionType
    {
       Payment,
       Transfer,
       Refund
    }

    public record class Transaction
    {
        public int Id { get; set; }
        public Guid SenderAcctId { get; set; }
        public Guid ReceiverAcctId { get; set; }
        public decimal? Amount { get; set; }
        public Status TransactionStatus { get; set; }
        public TransactionType TransactionType { get; set; }
        public string TransactionId { get; set; } = string.Empty;
        public DateTime? Timestamp {  get; set; }
        public int SenderIdNum { get; set; }
        public int ReceiverIdNum { get; set; }

    }
}
