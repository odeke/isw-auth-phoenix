using PhoenixAuth.BusinessLogic.Models;
using System.Threading.Tasks;

namespace PhoenixAuth.BusinessLogic.Services.CompleteRegistration
{
    public interface ICompleteRegistration
    {
        //Task<ResponseResult> CompleteClientRegistrationAsync(string otp);
        Task<ResponseResult> CompleteClientRegistrationAsync();
    }
}
