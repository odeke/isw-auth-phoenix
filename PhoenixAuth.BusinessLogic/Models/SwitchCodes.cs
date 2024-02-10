using System.ComponentModel;
using Newtonsoft.Json;

namespace PhoenixAuth.BusinessLogic.Models
{
    //[JsonConverter(typeof(ForceDefaultConverter))]
    public enum CustomCode
    {
        Success = 0,
        Failure = 1,
        Pending = 3
    }

    [JsonConverter(typeof(ForceDefaultConverter))]
    public enum SwitchCodes : long
    {
        [Description("TRANSACTION APPROVED")] Ok = 9000, //00
        [Description("BILLER NOT FOUND")] BillerNotFound = 70010, //10
        [Description("INVALID PAYMENT ITEM")] InvalidPaymentCode = 70017, //17
        [Description("INVALID OR DUPLICATE REQUEST REFERENCE")] InvalidRequestReference = 70018, //18
        [Description("DUPLICATE REQUEST REFERENCE")] DuplicateReference = 30020, //20
        [Description("BILLER  NOT ENABLED FOR CHANNEL")] BillerNotEnabledForChannel = 70026, //26
        [Description("BANK NOT ENABLED FOR BILLER")] BankNotEnabledForBiller = 70027, //27
        [Description("TERMINAL OWNER NOT ENABLED FOR BILLER")] TerminalOwnerNotEnabledForBiller = 70028, //28
        [Description("TERMINAL OWNER NOT ENABLED FOR CHANNEL")] TerminalOwnerNotEnabledForChannel = 70029, //29
        [Description("TERMINAL OWNER NOT SETUP OR ENABLED FOR BILL PAYMENT")] TerminalOwnerNotSetUp = 70030, //30
        [Description("UNRECOGNIZED CBN BANK CODE")] UNRECOGNISED_CBN_CODE = 70031, //31
        [Description("DATA NOT FOUND")] DATA_NOT_FOUND = 70038, //38
        [Description("TRANSACTION NOT PERMITTED ON TERMINAL")] TRANSACTION_NOT_PERMITTED_ON_TERMINAL = 70058, //58

        [Description("TRANSACTION APPROVED")] Success = 90000, //00
        [Description("END USER SERVICE DENIED")] ErrorResponse = 90006, //06
        [Description("REQUEST IN PROGRESS")] Pending = 90009, //09
        [Description("INVALID AMOUNT")] InvalidAmount = 90013, //13
        [Description("TRANSACTION DECLINED BY BILLER")] Failure = 90020, //20
        [Description("NON EXISTENT TRANSACTION")] NonexistentTransaction = 90025, //25
        [Description("FORMAT ERROR")] FormatError = 90030, //30
        [Description("INSUFFICIENT FUNDS")] InsufficientFunds = 90051, //51
        [Description("UN RECOGNIZABLE CUSTOMER NUMBER")] AccountNotFound = 90052, //52
        [Description("AUTHORIZATION ERROR")] AuthorizationError = 90063, //63
        [Description("AN ERROR OCCURED")] InternalError = 90096, //96
        [Description("BILLER TEMPORARILY UNAVAILABLE")] IssuerInoperative = 90091, //91
        [Description("ROUTING_ERROR")] RoutingError = 90092, //92
        [Description("DUPLICATE RECORD")] DuplicateRecord = 90094, //94



    }

}
