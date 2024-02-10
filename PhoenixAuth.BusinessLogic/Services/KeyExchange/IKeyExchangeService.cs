using System.Threading.Tasks;
using PhoenixAuth.BusinessLogic.Models;

namespace PhoenixAuth.BusinessLogic.Services.KeyExchange
{
    public interface IKeyExchangeService
    {
        Task<ResponseResult> KeyExchangeAsync();
    }
}
