namespace PhoenixAuth.BusinessLogic.Models
{
    public class QtPaymentNotification
    {
        public string RouteId { get; set; }
        public string ClientTerminalId { get; set; }
        public long ClientId { get; set; }
        public string IIN { get; set; }
        public PaymentDetail PaymentDetail { get; set; }
    }

    public class PhxBiller
    {
        public string Name { get; set; }
        public long Id { get; set; }
        public string FeesAccount { get; set; }
        public string TransactionAccount { get; set; }
        public string Code { get; set; }
        public bool PrepaidBiller { get; set; }
    }

    public class PaymentItem
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public string Code { get; set; }
        public string AlternateCode { get; set; }
        public string ServiceIdentifier { get; set; }
        public bool RePostingAllowed { get; set; }
        public bool ProcessedInRealtime { get; set; }
        public bool RemoteTranInquirySupported { get; set; }
        public int PendingTxnTimeThreshold { get; set; }
        public string CollectionAccountNo { get; set; }
        public string RouteId { get; set; }
        public string ProviderConnectionId { get; set; }
        public string NotificationEndpoint { get; set; }
    }

    public class PaymentDetail
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
        public string CustomerName { get; set; }
        public string ReceiptNo { get; set; }
        public string CollectionsAccount { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerPhoneNumber { get; set; }
        public string DepositorName { get; set; }
        public string ProductGroupCode { get; set; }
        public string Narration { get; set; }
        public string Otp { get; set; }
        public string Pin { get; set; }
        public string Units { get; set; }
        public string PaymentBreakDown { get; set; }
        public string PaymentValue { get; set; }
        public string TokenResponse { get; set; }
        public string TranType { get; set; }
        public string TransactionSet { get; set; }
        public string SourceOfFunds { get; set; }
        public bool FeeInclusiveInAmount { get; set; }
        public string TransactionLimitsCode { get; set; }
        public string WalletAccountId { get; set; }
        public string LimitCategoryCode { get; set; }
        public double ReversalAmount { get; set; }
        public bool RePostingAllowed { get; set; }
        public PhxBiller Biller { get; set; }
        public PaymentItem PaymentItem { get; set; }
    }









    //==================Response Object==================
    public class QtPaymentResponse
    {
        //public string CustomerToken { get; set; }
        //public string AdditionalData { get; set; }
        //public string ReceiptNo { get; set; }
        //public string Units { get; set; }
        //public string PaymentBreakDown { get; set; }
        //public string PaymentValue { get; set; }
        //public string TokenResponse { get; set; }
        //public string RequestReference { get; set; }

        public PaymentDetail PaymentDetail { get; set; }
    }

}
