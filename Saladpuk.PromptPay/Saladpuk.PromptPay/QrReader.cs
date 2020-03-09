using Saladpuk.Contracts.EMVCo;
using Saladpuk.Contracts.PromptPay.Models;
using Saladpuk.PromptPay.Models;
using System.Collections.Generic;
using System.Linq;
using ppay = Saladpuk.PromptPay.PromptPayValues;

namespace Saladpuk.PromptPay
{
    public class QrReader
    {
        private List<QrDataObject> segments = new List<QrDataObject>();

        public QrInfo Read(string qrCode)
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
        private (QrDataObject, string) extractSegments(QrDataObject data)
        {
            var currentValue = $"{data.Id}{data.LengthCode}{data.Value.Substring(0, data.Length)}";
            var firstSegment = new QrDataObject(currentValue);
            var other = data.RawValue.Substring(currentValue.Length);
            return (firstSegment, other);
        }

        private BillPayment getBillPayment()
        {
            var segments = extractSegment(ppay.BillPaymentTagId);
            if (!segments.Any())
            {
                return null;
            }

            var result = new BillPayment();
            foreach (var item in segments)
            {
                switch (item.Id)
                {
                    case ppay.AID:
                        result.DomesticMerchant = item.Value == ppay.DomesticMerchant;
                        break;
                    case ppay.BillderId:
                        const int SuffixLength = 2;
                        result.NationalIdOrTaxId = item.Value.Substring(0, item.Value.Length - SuffixLength);
                        result.Suffix = item.Value.Substring(item.Value.Length - SuffixLength);
                        break;
                    case ppay.Reference1:
                        result.Reference1 = item.Value;
                        break;
                    case ppay.Reference2:
                        result.Reference2 = item.Value;
                        break;
                    default: break;
                }
            }
            return result;
        }
        private CreditTransfer getCreditTransfer()
        {
            var segments = extractSegment(ppay.CreditTransferTagId);
            if (!segments.Any())
            {
                return null;
            }

            var result = new CreditTransfer();
            foreach (var item in segments)
            {
                switch (item.Id)
                {
                    case ppay.AID:
                        result.MerchantPresentedQR = item.Value == ppay.MerchantPresented;
                        break;
                    case ppay.MobileId:
                        const int SkipTagId = 2;
                        result.MobileNumber = item.Value.Substring(SkipTagId);
                        break;
                    case ppay.NationalOrTaxId:
                        result.NationalIdOrTaxId = item.Value;
                        break;
                    case ppay.EWalletId:
                        result.EWalletId = item.Value;
                        break;
                    case ppay.OTAId:
                        result.OTA = item.Value;
                        break;
                    default: break;
                }
            }
            return result;
        }
        private IEnumerable<QrDataObject> extractSegment(string specificTagId)
        {
            var merchant = segments.LastOrDefault(it => it.Identifier == QrIdentifier.MerchantAccountInformation);
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
