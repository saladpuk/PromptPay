using Saladpuk.Contracts;
using Saladpuk.Contracts.EMVCo;
using Saladpuk.Contracts.PromptPay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using emv = Saladpuk.Contracts.EMVCo.EMVCoValues;

namespace Saladpuk.PromptPay.Models
{
    public class QrInfo : IQrInfo
    {
        public IList<IQrDataObject> Segments { get; set; }

        public string PayloadFormatIndicator => getSegment(QrIdentifier.PayloadFormatIndicator);
        public string PointOfInitiationMethod => getSegment(QrIdentifier.PointOfInitiationMethod);
        public string MerchantAccountInformation => getSegment(QrIdentifier.MerchantAccountInformation);
        public string MerchantCategoryCode => getSegment(QrIdentifier.MerchantCategoryCode);
        public string TransactionCurrency => getSegment(QrIdentifier.TransactionCurrency);
        public string TransactionAmount => getSegment(QrIdentifier.TransactionAmount);
        public string TipOrConvenienceIndicator => getSegment(QrIdentifier.TipOrConvenienceIndicator);
        public string ValueOfConvenienceFeeFixed => getSegment(QrIdentifier.ValueOfConvenienceFeeFixed);
        public string ValueOfConvenienceFeePercentage => getSegment(QrIdentifier.ValueOfConvenienceFeePercentage);
        public string CountryCode => getSegment(QrIdentifier.CountryCode);
        public string MerchantName => getSegment(QrIdentifier.MerchantName);
        public string MerchantCity => getSegment(QrIdentifier.MerchantCity);
        public string PostalCode => getSegment(QrIdentifier.PostalCode);
        public string AdditionalData => getSegment(QrIdentifier.AdditionalData);
        public string CRC => getSegment(QrIdentifier.CRC);
        public string MerchantInformationLanguageTemplate => getSegment(QrIdentifier.MerchantInformationLanguageTemplate);
        public string RFU => getSegment(QrIdentifier.RFU);
        public string UnreservedTemplates => getSegment(QrIdentifier.UnreservedTemplates);

        public bool Reusable => (PointOfInitiationMethod ?? string.Empty) == emv.Static;
        public string Currency => Enum.TryParse(TransactionCurrency, out CurrencyCode currencyCode) ? currencyCode.ToString() : "undefine";

        public CreditTransfer CreditTransfer { get; set; }
        public BillPayment BillPayment { get; set; }

        public QrInfo(List<IQrDataObject> segments = null)
        {
            Segments = segments ?? new List<IQrDataObject>();
        }

        private string getSegment(QrIdentifier identifier)
            => Segments?.LastOrDefault(it => it.Identifier == identifier)?.Value ?? null;
    }
}
