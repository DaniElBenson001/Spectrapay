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
        private readonly INotificationService _notificationService;
        private readonly DataContext _context;

        public PaymentService(IHttpContextAccessor httpContextAccessor,INotificationService notificationService , DataContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _notificationService = notificationService;
            _context = context;
        }
        

        public async Task<DataResponse<NotificationMessageDTO>> MakeTransfer(PaymentDTO transfer)
        {
            DataResponse<NotificationMessageDTO> transferResponse = new();
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
                    SenderAcctId = SenderAcctIdToGuid,
                    ReceiverAcctId = ReceiverAcctIdToGuid,
                    Amount = transfer.Amount,
                    Timestamp = DateTime.Now,
                    TransactionType = TransactionType.Transfer,
                    TransactionStatus = Status.Pending,
                    SenderIdNum = SenderAcctData.Id,
                    ReceiverIdNum = ReceiverAcctdata.Id,
                    TransactionId = GenerateRandomString()
                };

                SenderAcctData.VirtualBalance -= transfer.Amount;
                ReceiverAcctdata.VirtualBalance += transfer.Amount;

                await _context.Transactions.AddAsync(newTransaction);

                newTransaction.TransactionStatus = Status.Successful;

                string senderMessage = $"You just sent {transfer.Amount} to {ReceiverAcctIdToGuid}";
                string receiverMessage = $"You just received {transfer.Amount} from {SenderAcctIdToGuid}";

                NotificationMessageDTO notifOutput = new()
                {
                    SenderMessage = senderMessage,
                    ReceiverMessage = receiverMessage,
                };

                await _notificationService.AddNotificationAsync(SenderAcctData.Id, senderMessage);
                await _notificationService.AddNotificationAsync(ReceiverAcctdata.Id, receiverMessage);

                await _context.SaveChangesAsync();

                transferResponse.Status = true;
                transferResponse.StatusMessage = "Transfer Successful";
                transferResponse.Data = notifOutput;
                return transferResponse;
            }
            catch(Exception ex)
            {
                transferResponse.Status = false;
                transferResponse.StatusMessage = $"Unsuccessful, An Error Occurred!{ex.Message}";
                return transferResponse;
            }
        }

        public string GenerateRandomString()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 16).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
