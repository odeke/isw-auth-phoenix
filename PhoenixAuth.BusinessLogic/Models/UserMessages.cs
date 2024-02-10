namespace PhoenixAuth.BusinessLogic.Models
{
    public static class UserMessages
    {
        //https://fluentvalidation.net/start
        public const string GreaterThan = "{PropertyName} must be greater than {ComparisonValue}";
        public const string GreaterThanOrEqualTo = "{PropertyName} must be greater than or equal to {ComparisonValue}";
        public const string LessThanOrEqualTo = "{PropertyName} must be less than or equal to {ComparisonValue}";
        public const string NotFound = "{0} with the specified Id is not found";
        //public const string PhoneNumberRegex = @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$";
        public const string PhoneNumberRegex = @"\(?\d{3}\)?-? *\d{3}-? *-?\d{4}";
        public const string WebsiteRegex = @"^(http|https):\/\/[\w\d]+\.[\w]+(\/[\w\d]+)$";
        public const string OnlyLetters = @"^[a-zA-Z ]+$";
        public const string Request = "URL: {url} with the following response: {result}.";
        public const string ErrorRequest = "URL: {url}\n Request: {request}. With the following response: {error}.";
        public const string InvalidValue = "{PropertyValue} is invalid for the \"{PropertyName}\" field";
        public const string CanNotBeNull = "{PropertyName} can not be {PropertyValue}";
        public const string EqualToStartDate = "{PropertyName} must be equal to the start date of the month";
        public const string Required = "{PropertyName} is required";
        public const string InvalidField = "{PropertyName} is invalid";
        public const string HomeCurrency = "The home currency is already defined";
        public const string EmptyEmailAddress = "Email address can not be empty";
        public const string EmptyPhoneNo = "Recipient number can not be empty";
        public const string RequestProcessingError = "An error has occurred while processing the request";
        public const string ExistMessage = "Type exit to close the program";
        public const string ExitProgramMsg = "exit";
        public const string NotFoundd = "{0}: {1} is not found";
        public const string OnlyLettersMsg = "{PropertyValue} is invalid for the \"{PropertyName}\" field. Only letters are allowed";
        public const string ErrorProcessing = "An error has occurred while processing this request. Please try again later";
        public const string InfoNotFound = "{0} Info is not found";
        public const string ProcessedSuccessfuly = "Transaction successful. TxnId: {0}";
        public const string ValidCustomer = "Valid customer";
        public const string InvalidCredentials = "Invalid Credentials";
        public const string PaymentRequest = "URL: {url}\nBody: {request}\n With the following response: {PaymentNotificationResponse}.";
        public const string TxnStatusRequest = "URL: {url}\nBody: {request}\n With the following response: {TransactionTempStore}.";
        public const string AlreadyExists = "{0} already exists";
        public const string SignatureVerification = "Processor signature validation failed";
        public const string FileNotFound = "File Not Found";
    }
}
