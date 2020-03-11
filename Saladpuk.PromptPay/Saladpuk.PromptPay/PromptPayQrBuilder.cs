using Saladpuk.EMVCo;
using Saladpuk.EMVCo.Contracts;
using Saladpuk.PromptPay.Contracts;
using Saladpuk.PromptPay.Contracts.Models;
using System.Linq;
using emv = Saladpuk.EMVCo.Contracts.EMVCoCodeConventions;
using ppay = Saladpuk.PromptPay.Contracts.PromptPayCodeConventions;

namespace Saladpuk.PromptPay
{
    // HACK: มันควรจะสืบทอดความสามารถของ QrBuilder ดีไหม?
    /// <summary>
    /// ตัวสร้าง QR PromptPay
    /// </summary>
    public class PromptPayQrBuilder : IPromptPayBuilder
    {
        #region Fields

        private QrBuilder qrBuilder;
        private BillPayment billPayment;
        private CreditTransfer creditTransfer;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// กำหนดค่าเริ่มต้นให้กับ ตัวสร้าง QR PromptPay
        /// </summary>
        public PromptPayQrBuilder()
        {
            qrBuilder = new QrBuilder();
            billPayment = new BillPayment();
            creditTransfer = new CreditTransfer();
        }

        #endregion Constructors

        #region IPromptPayBuilder members

        #region IEMVCo

        public IPromptPayBuilder Add(QrIdentifier identifier, string data)
        {
            qrBuilder.Add(identifier, data);
            return this;
        }

        public IPromptPayBuilder Add(string rawData)
        {
            qrBuilder.Add(rawData);
            return this;
        }

        public IPromptPayBuilder SetStaticQR()
        {
            qrBuilder.SetStaticQR();
            return this;
        }

        public IPromptPayBuilder SetDynamicQR()
        {
            qrBuilder.SetDynamicQR();
            return this;
        }

        public IPromptPayBuilder SetTransactionAmount(double amount)
        {
            qrBuilder.SetTransactionAmount(amount);
            return this;
        }

        public IPromptPayBuilder SetCountryCode(string code)
        {
            qrBuilder.SetCountryCode(code);
            return this;
        }

        public IPromptPayBuilder SetCurrencyCode(CurrencyCode code)
        {
            qrBuilder.SetCurrencyCode(code);
            return this;
        }

        public IPromptPayBuilder SetPayloadFormatIndicator(string version = "01")
        {
            qrBuilder.SetPayloadFormatIndicator(version);
            return this;
        }

        public IPromptPayBuilder SetCyclicRedundancyCheck(ICyclicRedundancyCheck crc)
        {
            qrBuilder.SetCyclicRedundancyCheck(crc);
            return this;
        }

        #endregion IEMVCo

        #region Credit transfer

        public string CreateCreditTransferQrCode()
            => CreateCreditTransferQrCode(creditTransfer);

        public string CreateCreditTransferQrCode(CreditTransfer transfer)
        {
            creditTransfer = transfer;

            var value = getValue();
            var digits = getLengthDigits(value);
            removeMerchantRecordsIfExists();
            Add($"{ppay.CreditTransferTag}{digits}{value}");
            return SetCyclicRedundancyCheck(new CRC16()).ToString();

            string getValue()
            {
                var aidRec = formatRecord(ppay.AIDTag, transfer.AID);
                var mobileRec = createMobileRecord();
                var receiverRec = string.IsNullOrWhiteSpace(transfer.NationalIdOrTaxId) ? string.Empty : formatRecord(ppay.NationalIdOrTaxIdTag, transfer.NationalIdOrTaxId);
                var eWalletRec = string.IsNullOrWhiteSpace(transfer.EWalletId) ? string.Empty : formatRecord(ppay.EWalletTag, transfer.EWalletId);
                var bankAccRed = string.IsNullOrWhiteSpace(transfer.BankAccount) ? string.Empty : formatRecord(ppay.BankAccountTag, transfer.BankAccount);
                var otaRec = string.IsNullOrWhiteSpace(transfer.OTA) ? string.Empty : formatRecord(ppay.OTATag, transfer.OTA);
                return $"{aidRec}{mobileRec}{receiverRec}{eWalletRec}{bankAccRed}{otaRec}";
            }
            string createMobileRecord()
            {
                if (string.IsNullOrWhiteSpace(transfer.MobileNumber))
                {
                    return string.Empty;
                }

                const string Prefix = "0066";
                var mobileNo = transfer.MobileNumber;
                if (!mobileNo.StartsWith(Prefix))
                {
                    // HACK: Stupid change prefix (refactor this later)
                    mobileNo = $"{Prefix}{mobileNo.Substring(1)}";
                }
                return formatRecord(ppay.MobileTag, mobileNo);
            }
        }

        public IPromptPayBuilder MobileNumber(string value)
        {
            creditTransfer.MobileNumber = value;
            return this;
        }

        public IPromptPayBuilder EWallet(string value)
        {
            creditTransfer.EWalletId = value;
            return this;
        }

        public IPromptPayBuilder BankAccount(string value)
        {
            creditTransfer.BankAccount = value;
            return this;
        }

        public IPromptPayBuilder OTA(string value)
        {
            creditTransfer.OTA = value;
            return this;
        }

        public IPromptPayBuilder MerchantPresentedQR()
        {
            creditTransfer.CustomerPresentedQR = false;
            return this;
        }

        public IPromptPayBuilder CustomerPresentedQR()
        {
            creditTransfer.CustomerPresentedQR = true;
            return this;
        }

        #endregion Credit transfer

        #region Bill payment

        public string CreateBillPaymentQrCode()
            => CreateBillPaymentQrCode(billPayment);

        public string CreateBillPaymentQrCode(BillPayment payment)
        {
            billPayment = payment;
            var value = getValue();
            var digits = getLengthDigits(value);
            removeMerchantRecordsIfExists();
            Add($"{ppay.BillPaymentTag}{digits}{value}");
            return SetCyclicRedundancyCheck(new CRC16()).ToString();

            string getValue()
            {
                const string DefaultSuffix = "00";
                payment.Suffix = payment.Suffix ?? DefaultSuffix;
                var aidSegment = formatRecord(ppay.AIDTag, payment.AID);
                var billerSegment = string.IsNullOrWhiteSpace(payment.BillerId) ? string.Empty : formatRecord(ppay.BillderIdTag, $"{payment.BillerId}{payment.Suffix}");
                var ref1Segment = string.IsNullOrWhiteSpace(payment.Reference1) ? string.Empty : formatRecord(ppay.Reference1Tag, payment.Reference1);
                var ref2Segment = string.IsNullOrWhiteSpace(payment.Reference2) ? string.Empty : formatRecord(ppay.Reference2Tag, payment.Reference2);
                return $"{aidSegment}{billerSegment}{ref1Segment}{ref2Segment}";
            }
        }

        public IPromptPayBuilder BillerSuffix(string value)
        {
            billPayment.Suffix = value;
            return this;
        }

        public IPromptPayBuilder BillRef1(string value)
        {
            billPayment.Reference1 = value;
            return this;
        }

        public IPromptPayBuilder BillRef2(string value)
        {
            billPayment.Reference2 = value;
            return this;
        }

        public IPromptPayBuilder DomesticMerchant()
        {
            billPayment.CrossBorderMerchantQR = false;
            return this;
        }

        public IPromptPayBuilder CrossBorderMerchant()
        {
            billPayment.CrossBorderMerchantQR = true;
            return this;
        }

        #endregion Bill payment

        public IPromptPayBuilder NationalId(string value)
        {
            creditTransfer.NationalIdOrTaxId = value;
            billPayment.BillerId = value;
            return this;
        }

        public IPromptPayBuilder TaxId(string value)
        {
            creditTransfer.NationalIdOrTaxId = value;
            billPayment.BillerId = value;
            return this;
        }

        public IPromptPayBuilder Amount(double amount)
            => SetTransactionAmount(amount);

        #endregion IPromptPayBuilder members

        #region Methods

        /// <summary>
        /// เรียกดูโค้ดทั้งหมดโดยไม่ระบุประเภทการใช้งาน
        /// </summary>
        public override string ToString()
        {
            var crcId = getIdCode(QrIdentifier.CRC);
            var crc = qrBuilder.QrDataObjects
                .Where(it => it.Id == crcId)
                .LastOrDefault()
                ?.RawValue ?? string.Empty;
            var qry = qrBuilder.QrDataObjects
                .Where(it => it.Id != crcId)
                .OrderBy(it => it.Id)
                .Select(it => it.RawValue)
                .Union(new[] { crc });
            return string.Join(string.Empty, qry);
        }

        private void removeMerchantRecordsIfExists()
        {
            var merchantIdentifierRange = emv.MerchantIdRange.Select(it => it.ToString());
            var merchantRecords = qrBuilder.QrDataObjects.Where(it => merchantIdentifierRange.Contains(it.Id)).ToList();
            foreach (var item in merchantRecords)
            {
                qrBuilder.QrDataObjects.Remove(item);
            }
        }

        private string formatRecord(string id, string value)
            => $"{id}{getLengthDigits(value)}{value}";

        private string getLengthDigits(string value)
            => value.Length.ToString("00");

        private string getIdCode(QrIdentifier identifier)
            => ((int)identifier).ToString("00");

        #endregion Methods
    }
}
