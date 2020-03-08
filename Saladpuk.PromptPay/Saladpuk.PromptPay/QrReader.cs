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
            var currentValue = $"{data.Id}{data.LengthCode}{data.Value[0..data.Length]}";
            var firstSegment = new QrDataObject(currentValue);
            var other = data.RawValue[currentValue.Length..^0];
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
                        result.NationalIdOrTaxId = item.Value[0..^2];
                        result.Suffix = item.Value[^2..^0];
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
                        result.MobileNumber = item.Value[2..^0];
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

            var tempValue = merchant.Value;
            while (!string.IsNullOrWhiteSpace(tempValue))
            {
                var (a, other) = extractSegments(new QrDataObject(tempValue));
                tempValue = other;
                yield return a;
            }
        }
    }
}
