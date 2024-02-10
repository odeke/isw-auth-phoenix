using Microsoft.AspNetCore.Mvc;
using PhoenixAuth.BusinessLogic.Models;
using PhoenixAuth.BusinessLogic.Services.ClientRegistration;
using PhoenixAuth.BusinessLogic.Services.KeyExchange;
using System.Threading.Tasks;
using PhoenixAuth.BusinessLogic.Services.CompleteRegistration;

namespace PhoenixAuth.Controllers
{
    /// <summary>
    /// Base Controller
    /// </summary>
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class IwsAuthController : ControllerBase
    {
        private readonly IClientRegistrationService _clientRegistration;
        private readonly IKeyExchangeService _exchangeService;
        private readonly ICompleteRegistration _completeRegistration;

        public IwsAuthController(IKeyExchangeService exchangeService, IClientRegistrationService clientRegistration, 
            ICompleteRegistration completeRegistration)
        {
            _exchangeService = exchangeService;
            _clientRegistration = clientRegistration;
            _completeRegistration = completeRegistration;
        }

        [Route("client_registration", Name = "ClientRegistration")]
        [HttpPost]
        [ProducesResponseType(typeof(ResponseResult), 200)]
        public async Task<IActionResult> ClientRegistration()
        {
            var result = await _clientRegistration.ClientRegistrationAsync();
            return Ok(result);
        }

        [Route("complete_registration", Name = "CompleteRegistration")]
        [HttpPost]
        [ProducesResponseType(typeof(ResponseResult), 200)]
        public async Task<IActionResult> CompleteRegistration()
        {
            var result = await _completeRegistration.CompleteClientRegistrationAsync();
            return Ok(result);
        }

        [Route("key_exchange", Name = "KeyExchange")]
        [HttpPost]
        [ProducesResponseType(typeof(ResponseResult), 200)]
        public async Task<IActionResult> KeyExchange()
        {
            var result = await _exchangeService.KeyExchangeAsync();
            return Ok(result);
        }
        
    }
}
