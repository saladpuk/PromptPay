using Saladpuk.PromptPay.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Saladpuk.PromptPay
{
    public class QrReader
    {
        private Dictionary<QrIdentifier, string> contents;

        public QrReader()
        {
            contents = new Dictionary<QrIdentifier, string>();
        }

        public QrInfo ConvertToQrInfo(string qrCode)
        {
            extractText(qrCode);
            return getQrInfo();
        }

        private void extractText(string text)
        {
            var data = new QrDataObject(text);
            var qrIdentifier = getIdentifier(data);
            var (value, other) = extractValueAndOtherData(data);
            contents.Add(qrIdentifier, value);
            if (!string.IsNullOrWhiteSpace(other))
            {
                extractText(other);
            }
        }
        private QrIdentifier getIdentifier(QrDataObject data)
        {
            if (!int.TryParse(data.Id, out int id))
            {
                throw new ArgumentException("QR identifier code isn't valid.");
            }
            var identifier = (QrIdentifier)id;
            if (identifier == QrIdentifier.Unknow)
            {
                throw new ArgumentException("QR identifier code isn't support.");
            }
            return identifier;
        }
        private int getLength(QrDataObject data)
        {
            if (!int.TryParse(data.Length, out int length))
            {
                throw new ArgumentException("QR identifier code isn't valid.");
            }
            return length;
        }
        private (string, string) extractValueAndOtherData(QrDataObject data)
        {
            var length = getLength(data);
            var value = data.Value[0..length];
            var other = data.Value[length..^0];
            return (value, other);
        }

        private QrInfo getQrInfo()
        {
            var qrInfo = new QrInfo();
            foreach (var item in contents)
            {
                switch (item.Key)
                {
                    case QrIdentifier.PayloadFormatIndicator:
                        qrInfo.PayloadFormatIndicator = item.Value;
                        break;
                    case QrIdentifier.PointOfInitiationMethod:
                        qrInfo.PointOfInitiationMethod = item.Value;
                        break;
                    case QrIdentifier.MerchantAccountInformation:
                        qrInfo.MerchantAccountInformation = item.Value;
                        break;
                    case QrIdentifier.MerchantCategoryCode:
                        qrInfo.MerchantCategoryCode = item.Value;
                        break;
                    case QrIdentifier.TransactionCurrency:
                        qrInfo.TransactionCurrency = item.Value;
                        break;
                    case QrIdentifier.TransactionAmount:
                        qrInfo.TransactionAmount = item.Value;
                        break;
                    case QrIdentifier.TipOrConvenienceIndicator:
                        qrInfo.TipOrConvenienceIndicator = item.Value;
                        break;
                    case QrIdentifier.ValueOfConvenienceFeeFixed:
                        qrInfo.ValueOfConvenienceFeeFixed = item.Value;
                        break;
                    case QrIdentifier.ValueOfConvenienceFeePercentage:
                        qrInfo.ValueOfConvenienceFeePercentage = item.Value;
                        break;
                    case QrIdentifier.CountryCode:
                        qrInfo.CountryCode = item.Value;
                        break;
                    case QrIdentifier.MerchantName:
                        qrInfo.MerchantName = item.Value;
                        break;
                    case QrIdentifier.MerchantCity:
                        qrInfo.MerchantCity = item.Value;
                        break;
                    case QrIdentifier.PostalCode:
                        qrInfo.PostalCode = item.Value;
                        break;
                    case QrIdentifier.AdditionalData:
                        qrInfo.AdditionalData = item.Value;
                        break;
                    case QrIdentifier.CRC:
                        qrInfo.CRC = item.Value;
                        break;
                    case QrIdentifier.MerchantInformationLanguageTemplate:
                        qrInfo.MerchantInformationLanguageTemplate = item.Value;
                        break;
                    case QrIdentifier.RFU:
                        qrInfo.RFU = item.Value;
                        break;
                    case QrIdentifier.UnreservedTemplates:
                        qrInfo.UnreservedTemplates = item.Value;
                        break;

                    default:
                    case QrIdentifier.Unknow:
                        break;
                }
            }
            return qrInfo;
        }
    }
}
