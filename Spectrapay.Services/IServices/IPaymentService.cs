using Spectrapay.Models.DtoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spectrapay.Services.IServices
{
    public interface IPaymentService
    {
        Task<DataResponse<string>> MakeTransaction(PaymentDTO transfer);
    }
}
