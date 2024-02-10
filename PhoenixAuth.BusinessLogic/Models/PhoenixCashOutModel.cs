using Newtonsoft.Json;
using PhoenixAuth.BusinessLogic.Extensions;

namespace PhoenixAuth.BusinessLogic.Models
{
    public class PhoenixCashOutModel
    {
        private double _amount;
        [JsonProperty("amount")] public double Amount
        {
            get => _amount.RoundTo(1);
            set => _amount = value;
        }

        [JsonProperty("surcharge")] public double Surcharge { get; set; }
        [JsonProperty("terminalId")] public string TerminalId { get; set; }
        [JsonProperty("requestReference")] public string RequestReference { get; set; }
        [JsonProperty("customerId")] public string CustomerId { get; set; }
        [JsonProperty("customerMobile")] public string CustomerMobile { get; set; }
        [JsonProperty("otp")] public string Otp { get; set; }
        [JsonProperty("paymentCode")] public string PaymentCode { get; set; }
        [JsonProperty("pin")] public string Pin { get; set; }
        [JsonProperty("alternateCustomerId")] public string AlternateCustomerId { get; set; }


        //"phoneNumber": "string",
        //"customerName": "string",
        //"sourceOfFunds": "string",
        //"narration": "string",
        //"depositorName": "string",
        //"location": "string",
        //"transactionCode": "string",
        //"customerToken": "string",
        //"additionalData": "string",
        //"collectionsAccountNumber": "string",
        //"currencyCode": "string",
    }

    public class PhoenixCashOutResponse
    {
        [JsonProperty("receiptNumber")] public string ReceiptNumber { get; set; }

        [JsonProperty("retrievalReference")] public string RetrievalReference { get; set; }

        [JsonProperty("transactionReference")] public string TransactionReference { get; set; }

        [JsonProperty("additionalData")] public string AdditionalData { get; set; }

        [JsonProperty("units")] public string Units { get; set; }

        [JsonProperty("paymentBreakDown")] public string PaymentBreakDown { get; set; }

        [JsonProperty("paymentValue")] public string PaymentValue { get; set; }

        [JsonProperty("paymentDate")] public string PaymentDate { get; set; }

        [JsonProperty("token")] public string Token { get; set; }

        [JsonProperty("amount")] public double Amount { get; set; }

        [JsonProperty("excise")] public double Excise { get; set; }

        [JsonProperty("surcharge")] public double Surcharge { get; set; }
        [JsonProperty("totalAmount")] public double TotalAmount { get; set; }

        [JsonProperty("tranType")] public string TranType { get; set; }
    }
}
