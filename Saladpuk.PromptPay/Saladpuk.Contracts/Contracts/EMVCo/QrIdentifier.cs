namespace Saladpuk.Contracts.EMVCo
{
    /// <summary>
    /// ประเภทข้อมูล
    /// </summary>
    public enum QrIdentifier
    {
        /// <summary>
        /// Version of the QR Code template.
        /// </summary>
        PayloadFormatIndicator = 0,
        /// <summary>
        /// Identifies the communication technology and whether the data is static or dynamic.
        /// </summary>
        /// <remarks>
        /// The value of "11" is used when the same QR Code is shown for more than one transaction.
        /// The value of "12" is used when a new QR Code is shown for each transaction.
        /// </remarks>
        PointOfInitiationMethod = 1,
        /// <summary>
        /// Identifies the merchant (02~51).
        /// </summary>
        /// <remarks>
        /// 02-03 are Reserved for Visa.
        /// 04-05 are Reserved for Mastercard
        /// 06-08 are Reserved by EMVCo
        /// 09-10 are Reserved for Discover
        /// 11-12 are Reserved for Amex
        /// 13-14 are Reserved for JCB
        /// 15-16 are Reserved for UnionPay
        /// 17-25 are Reserved by EMVCo
        /// 26-51 are Templates reserved for additional payment networks.
        /// </remarks>
        MerchantAccountInformation = 2,
        /// <summary>
        /// The Merchant Category Code (MCC) shall contain an MCC as defined by [ISO18245].
        /// </summary>
        MerchantCategoryCode = 52,
        /// <summary>
        /// Indicates the currency code of the transaction.
        /// </summary>
        TransactionCurrency = 53,
        /// <summary>
        /// The transaction amount, if known. If present, this value is displayed to the consumer
        /// by the mobile application when processing the transaction.
        /// If this data object is not present, the consumer is prompted to input the transaction
        /// amount to be paid to the merchant.
        /// </summary>
        TransactionAmount = 54,
        /// <summary>
        /// Indicates whether the consumer will be prompted to enter a tip or whether
        /// the merchant has determined that a flat, or percentage convenience fee is charged.
        /// </summary>
        TipOrConvenienceIndicator = 55,
        /// <summary>
        /// The fixed amount convenience fee.
        /// </summary>
        ValueOfConvenienceFeeFixed = 56,
        /// <summary>
        /// The percentage convenience fee.
        /// </summary>
        ValueOfConvenienceFeePercentage = 57,
        /// <summary>
        /// The Country Code shall contain a value as defined by [ISO 3166-1 alpha 2].
        /// </summary>
        CountryCode = 58,
        /// <summary>
        /// The Merchant Name shall be present.
        /// </summary>
        MerchantName = 59,
        /// <summary>
        /// The Merchant City shall be present.
        /// </summary>
        MerchantCity = 60,
        /// <summary>
        /// If present, the Postal Code should indicate the postal code of the merchant’s
        /// physical location. Depending on the country, the Postal code is the Zip code or
        /// PIN code or Postal code of the merchant.
        /// </summary>
        PostalCode = 61,
        AdditionalData = 62,
        /// <summary>
        /// Checksum calculated over all the data objects included in the QR Code.
        /// </summary>
        CRC = 63,
        /// <summary>
        /// The Merchant Information—Language Template includes merchant information in an alternate
        /// language and may use a character set different from the Common Character Set.
        /// It provides an alternative to the merchant information under the root. 
        /// </summary>
        MerchantInformationLanguageTemplate = 64,
        /// <summary>
        /// Reserved for Future Use (65~79).
        /// </summary>
        RFU = 65,
        UnreservedTemplates = 80,
    }
}
