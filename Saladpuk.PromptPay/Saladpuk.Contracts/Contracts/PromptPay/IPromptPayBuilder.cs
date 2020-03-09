using Saladpuk.Contracts.EMVCo;
using Saladpuk.Contracts.PromptPay.Models;

namespace Saladpuk.Contracts.PromptPay
{
    public interface IPromptPayBuilder : IEMVCoBuilder<IPromptPayBuilder>
    {
        IPromptPayBuilder SetBillPayment(BillPayment payment);
        IPromptPayBuilder SetCreditTransfer(CreditTransfer transfer);
    }
}
