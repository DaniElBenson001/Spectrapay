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
            List<TransactionsDTO> getTxns = new();
            int userId;

            try
            {
                userId = Convert.ToInt32(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier));

                var userTxnData = await _context.Transactions.Where(t => t.SenderIdNum == userId || t.ReceiverIdNum == userId).ToListAsync();

                foreach(var txn in userTxnData)
                {
                    if(txn.SenderIdNum != 0 && txn.ReceiverIdNum == userId)
                    {
                        getTxns.Add(new TransactionsDTO
                        {
                            AccountId = txn.ReceiverAcctId,
                            Amount = txn.Amount,
                            Timestamp = txn.Timestamp,
                            TxnType = txn.TransactionType,
                            TransferType = TransType.Credit,
                            TxnStatus = txn.TransactionStatus,
                            TxnId = txn.TransactionId,
                        });
                    }
                    if(txn.ReceiverIdNum != 0 && txn.SenderIdNum == userId)
                    {
                        getTxns.Add(new TransactionsDTO
                        {
                            AccountId = txn.SenderAcctId,
                            Amount = txn.Amount,
                            Timestamp = txn.Timestamp,
                            TxnType = txn.TransactionType,
                            TransferType = TransType.Debit,
                            TxnStatus = txn.TransactionStatus,
                            TxnId = txn.TransactionId
                        });
                    }
                }
                return getTxns;

            }
            catch (Exception)
            {
                return new List<TransactionsDTO>();
            }
        }

        public async Task<DataResponse<TransactionsGeneralDTO>> GetTransactionById(int Id)
        {
            DataResponse<TransactionsGeneralDTO> txnResponse = new();

            try
            {
                if(Id == 0)
                {
                    txnResponse.Status = false;
                    txnResponse.StatusMessage = "Transaction Not Found!";
                    return txnResponse;
                }

                var txnData = await _context.Transactions.Where(t => t.Id == Id).FirstOrDefaultAsync();

                TransactionsGeneralDTO txnOutput = new()
                {
                    SenderAcctId = txnData!.SenderAcctId,
                    ReceiverAcctId = txnData.ReceiverAcctId,
                    Amount = txnData.Amount,
                    Timestamp = txnData.Timestamp,
                    TxnType = txnData.TransactionType,
                    TxnStatus = txnData.TransactionStatus,
                    TxnId = txnData.TransactionId
                };

                txnResponse.Status = true;
                txnResponse.StatusMessage = "Successful";
                txnResponse.Data = txnOutput;
                return txnResponse;
            }
            catch(Exception)
            {
                txnResponse.Status = false;
                txnResponse.StatusMessage = "Unsuccessful, An Error Occured!";
                return txnResponse;
            }
        }

    }
}
