using Saladpuk.EMVCo;
using Saladpuk.EMVCo.Contracts;
using Saladpuk.EMVCo.Models;
using Saladpuk.PromptPay.Contracts;
using Saladpuk.PromptPay.Contracts.Models;
using Saladpuk.PromptPay.Models;
using System.Collections.Generic;
using System.Linq;
using ppay = Saladpuk.PromptPay.Contracts.PromptPayCodeConventions;

namespace Saladpuk.PromptPay
{
    /// <summary>
    /// ตัวอ่านข้อมูลจาก QR code ในรูปแบบ PromptPay
    /// </summary>
    public class PromptPayQrReader : QrReader, IPromptPayQrReader
    {
        #region Methods

        /// <summary>
        /// สร้างรายละเอียดของ QR
        /// </summary>
        /// <returns>รายละเอียดของ QR</returns>
        protected override IQrInfo CreateQrInfo()
            => new PromptPayQrInfo(Segments)
            {
                BillPayment = getBillPayment(),
                CreditTransfer = getCreditTransfer(),
            };

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
                        result.CrossBorderMerchantQR = item.Value == ppay.CrossBorderMerchant;
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
                    case ppay.BankAccountTag:
                        result.BankAccount = item.Value;
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
            var qry = Segments
                .Where(it => it.IdByConvention == QrIdentifier.MerchantAccountInformation && it.Id == specificTagId);
            var merchant = qry.LastOrDefault();
            var shouldSkip = merchant == null || merchant.Id != specificTagId;
            if (shouldSkip)
            {
                yield break;
            }

            var nextValue = merchant.Value;
            while (!string.IsNullOrWhiteSpace(nextValue))
            {
                var (segment, next) = ExtractSegments(new QrDataObject(nextValue));
                nextValue = next;
                yield return segment;
            }
        }

        #endregion Methods

        #region IPromptPayQrReader members

        /// <summary>
        /// แปลความหมายของข้อความให้อยู่ในรูปแบบ QR PromptPay
        /// </summary>
        /// <param name="code">รหัส QR code ที่ต้องการอ่าน</param>
        /// <returns>รายละเอียดของ QR</returns>
        public IPromptPayQrInfo ReadQrPromptPay(string code)
            => Read(code) as PromptPayQrInfo;

        #endregion IPromptPayQrReader members
    }
}
