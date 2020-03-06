namespace Saladpuk.PromptPay
{
    public enum QrIdentifier
    {
        PayloadFormatIndicator = 0,
        PointOfInitiationMethod = 1,
        MerchantAccountInformation = 2,
        MerchantCategoryCode = 52,
        TransactionCurrency = 53,
        TransactionAmount = 54,
        Tip = 55,
        ValueOfConvenienceFeeFixed = 56,
        ValueOfConvenienceFeePercentage = 57,
        CountryCode = 58,
        MerchantName = 59,
        MerchantCity = 60,
        PostalCode = 61,
        AdditionalData = 62,
        CRC = 63,
        MerchantInformationLanguageTemplate = 64,
        RFU = 65,
        UnreservedTemplates = 80,
    }
}
