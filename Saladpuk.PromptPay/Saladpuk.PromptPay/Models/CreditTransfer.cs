namespace Saladpuk.PromptPay.Models
{
    public class CreditTransfer
    {
        public string MobileNumber { get; set; }
        public string NationalIdOrTaxId { get; set; }
        public string EWalletId { get; set; }
        public string BankAccount { get; set; }
        /// <summary>
        /// Mandatory if Merchant-Presented QR
        /// </summary>
        public string OTA { get; set; }
        public bool MerchantPresentedQR { get; set; }

        public CreditTransfer(bool isMerchant = false, string ota = "")
        {
            MerchantPresentedQR = isMerchant;
            OTA = ota;
        }
    }
}
