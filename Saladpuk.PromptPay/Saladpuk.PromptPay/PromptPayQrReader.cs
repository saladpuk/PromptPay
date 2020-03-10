using Saladpuk.Contracts;
using Saladpuk.Contracts.EMVCo;
using Saladpuk.Contracts.PromptPay.Models;
using Saladpuk.PromptPay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using ppay = Saladpuk.Contracts.PromptPay.PromptPayCodeConventions;

namespace Saladpuk.PromptPay
{
    public class PromptPayQrReader : IQrReader
    {
        private List<IQrDataObject> segments = new List<IQrDataObject>();

        public IQrInfo Read(string qrCode)
        {
            extractText(qrCode);
            return new QrInfo(segments)
            {
                BillPayment = getBillPayment(),
                CreditTransfer = getCreditTransfer(),
            };
        }

        private void extractText(string text)
        {
            var reader = new QrDataObject(text);
            var (targetSegment, other) = extractSegments(reader);
            segments.Add(targetSegment);
            if (!string.IsNullOrWhiteSpace(other))
            {
                extractText(other);
            }
        }
        private (IQrDataObject, string) extractSegments(IQrDataObject data)
        {
            if (!int.TryParse(data.Length, out int length))
            {
                throw new ArgumentException("LengthCode isn't valid.");
            }
            var currentValue = $"{data.Id}{data.Length}{data.Value.Substring(0, length)}";
            var firstSegment = new QrDataObject(currentValue);
            var other = data.RawValue.Substring(currentValue.Length);
            return (firstSegment, other);
        }

        private BillPayment getBillPayment()
        {
            var segments = extractSegment(ppay.BillPaymentTag);
            if (!segments.Any())
            {
                return null;
            }

            var result = new BillPayment();
            foreach (var item in segments)
            {
                switch (item.Id)
                {
                    case ppay.AIDTag:
                        result.CrossBorderMerchantQR = item.Value == ppay.DomesticMerchant;
                        break;
                    case ppay.BillderIdTag:
                        const int SuffixLength = 2;
                        result.BillerId = item.Value.Substring(0, item.Value.Length - SuffixLength);
                        result.Suffix = item.Value.Substring(item.Value.Length - SuffixLength);
                        break;
                    case ppay.Reference1Tag:
                        result.Reference1 = item.Value;
                        break;
                    case ppay.Reference2Tag:
                        result.Reference2 = item.Value;
                        break;
                    default: break;
                }
            }
            return result;
        }
        private CreditTransfer getCreditTransfer()
        {
            var segments = extractSegment(ppay.CreditTransferTag);
            if (!segments.Any())
            {
                return null;
            }

            var result = new CreditTransfer();
            foreach (var item in segments)
            {
                switch (item.Id)
                {
                    case ppay.AIDTag:
                        result.CustomerPresentedQR = item.Value == ppay.CustomerPresented;
                        break;
                    case ppay.MobileTag:
                        const int SkipTagId = 2;
                        result.MobileNumber = item.Value.Substring(SkipTagId);
                        break;
                    case ppay.NationalIdOrTaxIdTag:
                        result.NationalIdOrTaxId = item.Value;
                        break;
                    case ppay.EWalletTag:
                        result.EWalletId = item.Value;
                        break;
                    case ppay.OTATag:
                        result.OTA = item.Value;
                        break;
                    default: break;
                }
            }
            return result;
        }
        private IEnumerable<IQrDataObject> extractSegment(string specificTagId)
        {
            var merchant = segments.LastOrDefault(it => it.IdByConvention == QrIdentifier.MerchantAccountInformation);
            var shouldSkip = merchant == null || merchant.Id != specificTagId;
            if (shouldSkip)
            {
                yield break;
            }

            var nextValue = merchant.Value;
            while (!string.IsNullOrWhiteSpace(nextValue))
            {
                var (segment, next) = extractSegments(new QrDataObject(nextValue));
                nextValue = next;
                yield return segment;
            }
        }
    }
}
