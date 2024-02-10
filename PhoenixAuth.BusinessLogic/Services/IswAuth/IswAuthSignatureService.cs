using PhoenixAuth.BusinessLogic.Models;

namespace PhoenixAuth.BusinessLogic.Services.IswAuth
{
    public class IswAuthSignatureService: IIswAuthSignatureService
    {
        public string PaymentNotificationSignature(PaymentDetail payDetails, string decryptedCustReference,
            string decryptedTerminalKey)
        {
            //TransactionDate + CustReference + Amount + Surcharge + Excise + PaymentItem.getId + RequestReference +
            //RetrievalReference + ProcessorCode + TerminalId

            var processorSignature =
                $"{payDetails.TransactionDate}{decryptedCustReference}{payDetails.Amount:0.0}{payDetails.Surcharge:0.0}" +
                $"{payDetails.Excise:0.0}{payDetails.PaymentItem.Id}{payDetails.RequestReference}" +
                $"{payDetails.RetrievalReference}{payDetails.ProcessorCode}{decryptedTerminalKey}";

            return processorSignature;
        }

        public string ValidationSignature(CustomerDetail customerDetail, string decryptedCustReference,
            string decryptedTerminalKey)
        {
            //TransactionDate + CustReference + Amount + RequestReference + RetrievalReference + ProcessorCode + TerminalId

            var processorSignature =
                $"{customerDetail.TransactionDate}{decryptedCustReference}{customerDetail.Amount:0.0}" +
                $"{customerDetail.RequestReference}{customerDetail.RetrievalReference}{customerDetail.ProcessorCode}" +
                $"{decryptedTerminalKey}";
            return processorSignature;
        }
    }
}
