using EskroAfrica.PaymentService.Common.DTOs.Paystack;

namespace EskroAfrica.PaymentService.Application.Interfaces
{
    public interface IPaystackService
    {
        Task<PaystackResponse<InitiateTransactionResponse>> InitiateTransaction(InitiateTransactionRequest request);
        Task<PaystackResponse<VerifyTransactionResponse>> VerifyTransaction(string reference);
    }
}
