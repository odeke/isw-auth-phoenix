using PhoenixAuth.BusinessLogic.Models;

namespace PhoenixAuth.BusinessLogic.Services.IswAuth
{
    public interface IIswAuthValidationService
    {
        /// <summary>
        /// Does data decryption using a session Key and signature verification
        /// </summary>
        /// <param name="model">Payment notification object</param>
        /// <param name="authHeaders">Authentication headers i.e Both Transport keys and caller signature </param>
        /// <returns>Decrypted customerRef and TerminalId</returns>
        (string decryptedCustReference, string decryptedTerminalKey) ValidationAuth(QtAccountValidation model, PhoenixAuthHeaders authHeaders);
    }
}
