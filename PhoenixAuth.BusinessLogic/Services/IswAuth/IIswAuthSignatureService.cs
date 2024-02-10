using PhoenixAuth.BusinessLogic.Models;

namespace PhoenixAuth.BusinessLogic.Services.IswAuth
{
    public interface IIswAuthSignatureService
    {
        /// <summary>
        /// Generate payment notification processor signature
        /// </summary>
        /// <param name="payDetails"></param>
        /// <param name="decryptedCustReference"></param>
        /// <param name="decryptedTerminalKey"></param>
        /// <returns></returns>
        string PaymentNotificationSignature(PaymentDetail payDetails, string decryptedCustReference,
            string decryptedTerminalKey);

        /// <summary>
        /// Generate validation processor signature
        /// </summary>
        /// <param name="customerDetail"></param>
        /// <param name="decryptedCustReference"></param>
        /// <param name="decryptedTerminalKey"></param>
        /// <returns></returns>
        string ValidationSignature(CustomerDetail customerDetail, string decryptedCustReference,
            string decryptedTerminalKey);
    }
}
