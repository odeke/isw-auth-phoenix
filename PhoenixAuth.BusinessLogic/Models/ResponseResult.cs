using PhoenixAuth.BusinessLogic.Extensions;

namespace PhoenixAuth.BusinessLogic.Models
{
    public class ResponseResult//<T> where T : class 
    {
        private string _msg = string.Empty;
        public SwitchCodes ResponseCode { get; set; } = SwitchCodes.Success;
        public string ResponseMessage
        {
            get => string.IsNullOrWhiteSpace(_msg) ? ResponseCode.GetDescription() : _msg;
            set => _msg = value;
        }

        public string RequestReference { get; set; }

        public object Response { get; set; }

    }
}
