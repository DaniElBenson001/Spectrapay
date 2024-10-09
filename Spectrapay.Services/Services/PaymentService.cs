using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Spectrapay.Models.DtoModels;
using Spectrapay.Models.Entities;
using Spectrapay.Services.Data;
using Spectrapay.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Spectrapay.Services.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;

        public PaymentService(IHttpContextAccessor httpContextAccessor, DataContext context, IConfiguration configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public async Task<DataResponse<string>> MakeTransaction(PaymentDTO transfer)
        {
            var transferResponse = new DataResponse<string>();
            int userId;

            try
            {
                userId = Convert.ToInt32(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier));

                if(_httpContextAccessor.HttpContext == null)
                {
                    transferResponse.Status = false;
                    transferResponse.StatusMessage = "User Not Found";
                    return transferResponse;
                }

                var SenderAcctIdToGuid = Guid.Parse(transfer.SenderAcctId);
                var SenderAcctData = await _context.Users.Where(u => u.AccountId == SenderAcctIdToGuid).FirstAsync();

                if(SenderAcctData == null)
                {
                    transferResponse.Status = false;
                    transferResponse.StatusMessage = "Sender Account Not Found";
                    return transferResponse;
                }

                var ReceiverAcctIdToGuid = Guid.Parse(transfer.ReceivcerAccId);
                var ReceiverAcctdata = await _context.Users.Where(u => u.AccountId == ReceiverAcctIdToGuid).FirstAsync();

                if(ReceiverAcctdata == null)
                {
                    transferResponse.Status = false;
                    transferResponse.StatusMessage = "Account Not Found";
                    return transferResponse;
                }

                if(SenderAcctIdToGuid == ReceiverAcctIdToGuid)
                {
                    transferResponse.Status = false;
                    transferResponse.StatusMessage = "Do not send Funds to yourself!";
                    return transferResponse;
                }

                if(SenderAcctData.VirtualBalance < transfer.Amount)
                {
                    transferResponse.Status = false;
                    transferResponse.StatusMessage = "Insufficient Funds";
                    return transferResponse;
                }

                if(transfer.Amount <= 0)
                {
                    transferResponse.Status = false;
                    transferResponse.StatusMessage = "You cannot send Funds zero or below";
                    return transferResponse;
                }

                Transaction newTransaction = new()
                {
                    SenderId = SenderAcctIdToGuid,
                    ReceiverId = ReceiverAcctIdToGuid,
                    Amount = transfer.Amount,
                    Timestamp = DateTime.Now,
                    TransactionType = TransactionType.Transfer,
                    TransactionStatus = Status.Pending,
                };

                SenderAcctData.VirtualBalance -= transfer.Amount;
                ReceiverAcctdata.VirtualBalance += transfer.Amount;

                await _context.Transactions.AddAsync(newTransaction);

                newTransaction.TransactionStatus = Status.Successful;

                await _context.SaveChangesAsync();

                transferResponse.Status = true;
                transferResponse.StatusMessage = "Transfer Successful";
                return transferResponse;
            }
            catch(Exception)
            {
                transferResponse.Status = false;
                transferResponse.StatusMessage = "Unsuccessful, An Error Occurred!";
                return transferResponse;
            }
        }
    }
}
