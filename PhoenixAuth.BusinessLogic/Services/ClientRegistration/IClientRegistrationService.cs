using System.Threading.Tasks;
using PhoenixAuth.BusinessLogic.Models;

namespace PhoenixAuth.BusinessLogic.Services.ClientRegistration
{
    public interface IClientRegistrationService
    {
        Task<ResponseResult> ClientRegistrationAsync();
    }
}
