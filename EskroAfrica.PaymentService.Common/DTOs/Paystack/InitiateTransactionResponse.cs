namespace EskroAfrica.PaymentService.Common.DTOs.Paystack
{
    public class InitiateTransactionResponse
    {
        public string authauthorization_url { get; set; }
        public string access_code { get; set; }
        public string reference { get; set; }
    }
}
