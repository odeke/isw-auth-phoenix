using System.Threading.Tasks;
using PhoenixAuth.BusinessLogic.Models;

namespace PhoenixAuth.BusinessLogic.Services.CashOut
{
    public interface ICashOutService
    {
        Task<ResponseResult> CashWithdrawAsync(PhoenixCashOutModel model);
    }
}
