using emv = Saladpuk.PromptPay.EMVCoValues;

namespace Saladpuk.PromptPay.Models
{
    public class QrInfo
    {
        public string PayloadFormatIndicator { get; set; }
        public string PointOfInitiationMethod { get; set; }
        public string MerchantAccountInformation { get; set; }
        public string MerchantCategoryCode { get; set; }
        public string TransactionCurrency { get; set; }
        public string TransactionAmount { get; set; }
        public string TipOrConvenienceIndicator { get; set; }
        public string ValueOfConvenienceFeeFixed { get; set; }
        public string ValueOfConvenienceFeePercentage { get; set; }
        public string CountryCode { get; set; }
        public string MerchantName { get; set; }
        public string MerchantCity { get; set; }
        public string PostalCode { get; set; }
        public string AdditionalData { get; set; }
        public string CRC { get; set; }
        public string MerchantInformationLanguageTemplate { get; set; }
        public string RFU { get; set; }
        public string UnreservedTemplates { get; set; }

        public bool Reusable => (PointOfInitiationMethod ?? string.Empty) == emv.Static;
    }
}
