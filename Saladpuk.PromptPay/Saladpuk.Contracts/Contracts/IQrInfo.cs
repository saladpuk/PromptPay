using System.Collections.Generic;

namespace Saladpuk.Contracts
{
    public interface IQrInfo
    {
        IList<IQrDataObject> Segments { get; set; }
        string PayloadFormatIndicator { get; }
        string PointOfInitiationMethod { get; }
        string MerchantAccountInformation { get; }
        string MerchantCategoryCode { get; }
        string TransactionCurrency { get; }
        string TransactionAmount { get; }
        string TipOrConvenienceIndicator { get; }
        string ValueOfConvenienceFeeFixed { get; }
        string ValueOfConvenienceFeePercentage { get; }
        string CountryCode { get; }
        string MerchantName { get; }
        string MerchantCity { get; }
        string PostalCode { get; }
        string AdditionalData { get; }
        string CRC { get; }
        string MerchantInformationLanguageTemplate { get; }
        string RFU { get; }
        string UnreservedTemplates { get; }
    }
}
