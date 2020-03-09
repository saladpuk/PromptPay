namespace Saladpuk.Contracts.PromptPay.Models
{
    public class CreditTransfer
    {
        public string MobileNumber { get; set; }
        public string NationalIdOrTaxId { get; set; }
        public string EWalletId { get; set; }
        public string BankAccount { get; set; }
        public string OTA { get; set; }
        public bool MerchantPresentedQR { get; set; }
    }
}
