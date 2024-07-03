namespace EskroAfrica.PaymentService.Common.DTOs.Paystack
{
    public class InitiateTransactionRequest
    {
        public string Amount { get; set; }
        public string Email { get; set; }
    }
}
