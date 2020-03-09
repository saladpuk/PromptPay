using Saladpuk.Contracts.EMVCo;
using Saladpuk.Contracts.PromptPay.Models;

namespace Saladpuk.Contracts.PromptPay
{
    public interface IPromptPayBuilder : IEMVCoBuilder<IPromptPayBuilder>
    {
        #region Credit transfer

        string GetCreditTransferQR();
        string GetCreditTransferQR(CreditTransfer transfer);
        IPromptPayBuilder MobileNumber(string value);
        IPromptPayBuilder EWallet(string value);
        IPromptPayBuilder BankAccount(string value);
        IPromptPayBuilder OTA(string value);
        IPromptPayBuilder MerchantPresentedQR();
        IPromptPayBuilder CustomerPresentedQR();

        #endregion Credit transfer

        #region Biller

        string GetBillPaymentQR();
        string GetBillPaymentQR(BillPayment payment);
        IPromptPayBuilder BillerSuffix(string value);
        IPromptPayBuilder BillRef1(string value);
        IPromptPayBuilder BillRef2(string value);
        IPromptPayBuilder DomesticMerchant();
        IPromptPayBuilder CrossBorderMerchant();

        #endregion Biller

        IPromptPayBuilder NationalId(string value);
        IPromptPayBuilder TaxId(string value);
        IPromptPayBuilder Amount(double amount);
    }
}
