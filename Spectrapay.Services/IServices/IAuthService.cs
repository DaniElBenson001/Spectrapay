using Spectrapay.Models.DtoModels;
using Spectrapay.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spectrapay.Services.IServices
{
    public interface IAuthService
    {
        Task<DataResponse<LoginView>> Login(LoginUserDTO request);
    }
}
