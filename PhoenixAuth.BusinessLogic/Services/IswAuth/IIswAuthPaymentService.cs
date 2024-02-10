using PhoenixAuth.BusinessLogic.Models;

namespace PhoenixAuth.BusinessLogic.Services.IswAuth
{
    public interface IIswAuthPaymentService
    {
        /// <summary>
        /// Does data decryption using a session Key and signature verification
        /// </summary>
        /// <param name="model">Payment notification object</param>
        /// <param name="authHeaders">Authentication headers i.e Both Transport keys and caller signature </param>
        /// <returns></returns>
        (string decryptedCustReference, string decryptedWalletAccountId, string decryptedTerminalKey)
            PaymentNotificationAuth(QtPaymentNotification model, PhoenixAuthHeaders authHeaders);
    }
}
