using Spectrapay.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spectrapay.Models.DtoModels
{
    public enum TransType
    {
        Credit,
        Debit
    }
    public record class TransactionsDTO
    {
        public Guid AccountId { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? Timestamp { get; set; }
        public TransactionType TxnType { get; set; }
        public TransType TransferType { get; set; }
        public Status TxnStatus { get; set; }
        public string TxnId { get; set; } = string.Empty;
        public Guid SenderAcctId { get; set; }
        public Guid ReceiverAcctId { get; set;}

    }

    public record class TransactionsGeneralDTO
    {
        public decimal? Amount { get; set; }
        public DateTime? Timestamp { get; set; }
        public TransactionType TxnType { get; set; }
        public TransType TransferType { get; set; }
        public Status TxnStatus { get; set; }
        public string TxnId { get; set; } = string.Empty;
        public Guid SenderAcctId { get; set; }
        public Guid ReceiverAcctId { get; set; }
    }
}
