using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spectrapay.Models.Entities
{
    public enum RefundStatus
    {
        Approved,
        Pending,
        Denied
    }
    public record class Refund
    {
        public int Id { get; set; }
        public Guid OriginalTransactionId { get; set; }
        public Guid RequesterId { get; set; }
        public decimal? Amount { get; set; }
        public RefundStatus RefundStatus { get; set; }
        public DateTime? RequestedAt { get; set; }
        public DateTime? ProcessedAt { get; set; }
    }
}
