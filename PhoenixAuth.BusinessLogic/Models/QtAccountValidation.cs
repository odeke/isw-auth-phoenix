namespace PhoenixAuth.BusinessLogic.Models
{
    public class QtAccountValidation
    {
        public string RouteId { get; set; }
        public string ClientTerminalId { get; set; }
        public int ClientId { get; set; }
        public string IIN { get; set; }
        public CustomerDetail CustomerDetail { get; set; }
    }


    public class CustomerDetail
    {
        public string RequestReference { get; set; }
        public string Stan { get; set; }
        public string RetrievalReference { get; set; }
        public string TerminalId { get; set; }
        public string Issuer { get; set; }
        public string Teller { get; set; }
        public string ProcessorCode { get; set; }
        public string AccountName { get; set; }
        public string TransactionDate { get; set; }
        public string CurrencyCode { get; set; }
        public string Location { get; set; }
        public string CommissionAccountId { get; set; }
        public string CustReference { get; set; }
        public double Amount { get; set; }
        public string TransactionReference { get; set; }
        public string ThirdPartyCode { get; set; }
        public string TransactionCode { get; set; }
        public string AlternateCustReference { get; set; }
        public string CustomerToken { get; set; }
        public string AdditionalData { get; set; }
        public double Surcharge { get; set; }
        public double Excise { get; set; }
        public string Region { get; set; }
        public string SplitBridgeRouterGroup { get; set; }
        public long TransactionId { get; set; }
        public PhxBiller Biller { get; set; }
        public PaymentItem PaymentItem { get; set; }
    }


    //===========================Response Object=============
    public class QtValidationResponse
    {
        public string ThirdPartyCode { get; set; }
        public string CustReference { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public string AlternateCustReference { get; set; }
        public string AdditionalData { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string CollectionsAccountNumber { get; set; }
        public string BalanceNarration { get; set; }
        public string BalanceType { get; set; }
        public bool DisplayBalance { get; set; }
        public double Balance { get; set; }
        public double Surcharge { get; set; }
        public double Excise { get; set; }
        public bool AmountFixed { get; set; }
        public string CustomerToken { get; set; }
        public string PhoneNumber { get; set; }
        public string Narration { get; set; }
        public PhxBiller Biller { get; set; }
        public PaymentItem PaymentItem { get; set; }
        public bool FeeInclusiveInAmount { get; set; }
    }

}
