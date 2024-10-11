using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Spectrapay.Models.DtoModels;
using Spectrapay.Services.Data;
using Spectrapay.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Spectrapay.Services.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataContext _context;

        public TransactionService(DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<TransactionsDTO>> GetTransactionHistory()
        {
            List<TransactionsDTO> getTransactions = new();
            int userId;

            try
            {
                userId = Convert.ToInt32(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier));

                var userTransactionsData = await _context.Transactions.Where(t => t.SenderIdNum == userId || t.ReceiverIdNum == userId).ToListAsync();

                foreach(var txn in userTransactionsData)
                {
                    if(txn.SenderIdNum != 0 && txn.ReceiverIdNum == userId)
                    {
                        getTransactions.Add(new TransactionsDTO
                        {
                            AccountId = txn.ReceiverAcctId,
                            Amount = txn.Amount,
                            Timestamp = txn.Timestamp,
                            TxnType = txn.TransactionType,
                            TransferType = TransType.Credit,
                            TxnStatus = txn.TransactionStatus
                        });
                    }
                    if(txn.ReceiverIdNum != 0 && txn.SenderIdNum == userId)
                    {
                        getTransactions.Add(new TransactionsDTO
                        {
                            AccountId = txn.SenderAcctId,
                            Amount = txn.Amount,
                            Timestamp = txn.Timestamp,
                            TxnType = txn.TransactionType,
                            TransferType = TransType.Debit,
                            TxnStatus = txn.TransactionStatus
                        });
                    }
                }
                return getTransactions;

            }
            catch (Exception)
            {
                return new List<TransactionsDTO>();
            }
        }


    }
}
