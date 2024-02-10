using Microsoft.AspNetCore.Mvc;
using PhoenixAuth.BusinessLogic.Models;
using System.Threading.Tasks;
using PhoenixAuth.BusinessLogic.Services.CashOut;

namespace PhoenixAuth.Controllers
{
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {

        private readonly ICashOutService _cashOut;

        public TransactionsController(ICashOutService cashOut)
        {
            _cashOut = cashOut;
        }

        [Route("cash_out", Name = "CashOut")]
        [HttpPost]
        [ProducesResponseType(typeof(ResponseResult), 200)]
        public async Task<IActionResult> CashOut([FromBody] PhoenixCashOutModel model)
        {
            var result = await _cashOut.CashWithdrawAsync(model);
            return Ok(result);
        }
    }
}
