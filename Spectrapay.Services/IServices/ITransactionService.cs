﻿using Spectrapay.Models.DtoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spectrapay.Services.IServices
{
    public interface ITransactionService
    {
        Task<List<TransactionsDTO>> GetTransactionHistory();
        Task<DataResponse<TransactionsGeneralDTO>> GetTransactionById(int Id);
    }
}
