using Saladpuk.Contracts;
using Saladpuk.Contracts.EMVCo;
using Saladpuk.Contracts.PromptPay;
using Saladpuk.Contracts.PromptPay.Models;
using Saladpuk.PromptPay.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using emv = Saladpuk.Contracts.EMVCo.EMVCoValues;
using ppay = Saladpuk.Contracts.PromptPay.PromptPayValues;

namespace Saladpuk.PromptPay
{
    public class QrBuilder : IPromptPayBuilder
    {
        internal BillPayment billPayment;
        internal CreditTransfer creditTransfer;
        private readonly List<QrDataObject> qrDataObjects;

        public QrBuilder()
        {
            billPayment = new BillPayment();
            creditTransfer = new CreditTransfer();
            qrDataObjects = new List<QrDataObject>();
        }

        #region IEMVCo

        public IPromptPayBuilder Add(QrIdentifier identifier, string data)
        {
            if (string.IsNullOrWhiteSpace(data))
            {
                throw new ArgumentException("Invalid data");
            }

            var id = getIdCode(identifier);
            var digits = getLengthDigits(data ?? string.Empty);
            removeOldRecordIfExists(id);
            qrDataObjects.Add(new QrDataObject($"{id}{digits}{data}"));
            return this;
        }

        public IPromptPayBuilder Add(string rawData)
        {
            var isArgumentValid = !string.IsNullOrWhiteSpace(rawData)
                && rawData.Length >= emv.MinContentLength;
            if (!isArgumentValid)
            {
                throw new ArgumentException("Invalid data");
            }

            var data = new QrDataObject(rawData);
            removeOldRecordIfExists(data.Id);
            qrDataObjects.Add(data);
            return this;
        }

        public IPromptPayBuilder SetStaticQR()
            => Add(QrIdentifier.PointOfInitiationMethod, emv.Static);

        public IPromptPayBuilder SetDynamicQR()
            => Add(QrIdentifier.PointOfInitiationMethod, emv.Dynamic);

        public IPromptPayBuilder SetTransactionAmount(double amount)
            => Add(QrIdentifier.TransactionAmount, Math.Abs(amount).ToString("0.00"));

        public IPromptPayBuilder SetCountryCode(string code)
            => Add(QrIdentifier.CountryCode, new RegionInfo(code).TwoLetterISORegionName);

        public IPromptPayBuilder SetCurrencyCode(CurrencyCode code)
            => Add(QrIdentifier.TransactionCurrency, ((int)code).ToString("000"));

        public IPromptPayBuilder SetPayloadFormatIndicator(string version = "01")
            => Add(QrIdentifier.PayloadFormatIndicator, version);

        public IPromptPayBuilder SetCyclicRedundancyCheck(ICyclicRedundancyCheck crc)
        {
            var id = getIdCode(QrIdentifier.CRC);
            var currentCode = $"{ToString()}{id}{emv.CRCDigits.ToString("00")}";
            var computedValue = crc.ComputeChecksum(currentCode);
            Add(QrIdentifier.CRC, computedValue);
            return this;
        }

        #endregion IEMVCo

        #region Credit transfer

        public string GetCreditTransferQR()
            => GetCreditTransferQR(creditTransfer);

        public string GetCreditTransferQR(CreditTransfer transfer)
        {
            creditTransfer = transfer;
            billPayment.NationalIdOrTaxId = creditTransfer.NationalIdOrTaxId;

            var value = getValue();
            var digits = getLengthDigits(value);
            removeMerchantRecordsIfExists();
            Add($"{ppay.CreditTransferTagId}{digits}{value}");
            return SetCyclicRedundancyCheck(new SimpleCRC16()).ToString();

            string getValue()
            {
                var aidRec = formatRecord(ppay.AID, transfer.MerchantPresentedQR ? ppay.MerchantPresented : ppay.CustomerPresented);
                var mobileRec = createMobileRecord();
                var receiverRec = string.IsNullOrWhiteSpace(transfer.NationalIdOrTaxId) ? string.Empty : formatRecord(ppay.NationalOrTaxId, transfer.NationalIdOrTaxId);
                var eWalletRec = string.IsNullOrWhiteSpace(transfer.EWalletId) ? string.Empty : formatRecord(ppay.EWalletId, transfer.EWalletId);
                var bankAccRed = string.IsNullOrWhiteSpace(transfer.BankAccount) ? string.Empty : formatRecord(ppay.BankAccountId, transfer.BankAccount);
                var otaRec = string.IsNullOrWhiteSpace(transfer.OTA) ? string.Empty : formatRecord(ppay.OTAId, transfer.OTA);
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
                return formatRecord(ppay.MobileId, mobileNo);
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
            creditTransfer.MerchantPresentedQR = true;
            return this;
        }

        public IPromptPayBuilder CustomerPresentedQR()
        {
            creditTransfer.MerchantPresentedQR = false;
            return this;
        }

        #endregion Credit transfer

        #region Billder

        public string GetBillPaymentQR()
            => GetBillPaymentQR(billPayment);

        public string GetBillPaymentQR(BillPayment payment)
        {
            billPayment = payment;
            creditTransfer.NationalIdOrTaxId = payment.NationalIdOrTaxId;

            var value = getValue();
            var digits = getLengthDigits(value);
            removeMerchantRecordsIfExists();
            Add($"{ppay.BillPaymentTagId}{digits}{value}");
            return SetCyclicRedundancyCheck(new SimpleCRC16()).ToString();

            string getValue()
            {
                payment.Suffix = payment.Suffix ?? "00";
                var aidRec = formatRecord(ppay.AID, payment.DomesticMerchant ? ppay.DomesticMerchant : ppay.CrossBorderMerchant);
                var billerRec = string.IsNullOrWhiteSpace(payment.NationalIdOrTaxId) ? string.Empty : formatRecord(ppay.BillderId, $"{payment.NationalIdOrTaxId}{payment.Suffix}");
                var ref1Rec = string.IsNullOrWhiteSpace(payment.Reference1) ? string.Empty : formatRecord(ppay.Reference1, payment.Reference1);
                var ref2Rec = string.IsNullOrWhiteSpace(payment.Reference2) ? string.Empty : formatRecord(ppay.Reference2, payment.Reference2);
                return $"{aidRec}{billerRec}{ref1Rec}{ref2Rec}";
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
            billPayment.DomesticMerchant = true;
            return this;
        }

        public IPromptPayBuilder CrossBorderMerchant()
        {
            billPayment.DomesticMerchant = false;
            return this;
        }

        #endregion Billder

        public IPromptPayBuilder NationalId(string value)
        {
            creditTransfer.NationalIdOrTaxId = value;
            billPayment.NationalIdOrTaxId = value;
            return this;
        }

        public IPromptPayBuilder TaxId(string value)
        {
            creditTransfer.NationalIdOrTaxId = value;
            billPayment.NationalIdOrTaxId = value;
            return this;
        }

        public IPromptPayBuilder Amount(double amount)
            => SetTransactionAmount(amount);

        private void removeMerchantRecordsIfExists()
        {
            var merchantIdentifierRange = Enumerable.Range(2, 50).Select(it => it.ToString());
            var merchantRecords = qrDataObjects.Where(it => merchantIdentifierRange.Contains(it.Id)).ToList();
            foreach (var item in merchantRecords)
            {
                qrDataObjects.Remove(item);
            }
        }

        private string formatRecord(string id, string value)
            => $"{id}{getLengthDigits(value)}{value}";

        private void removeOldRecordIfExists(string id)
        {
            var selected = qrDataObjects.FirstOrDefault(it => it.Id == id);
            if (selected != null)
            {
                qrDataObjects.Remove(selected);
            }
        }

        private string getLengthDigits(string value)
            => value.Length.ToString("00");

        private string getIdCode(QrIdentifier identifier)
            => ((int)identifier).ToString("00");

        public override string ToString()
        {
            var crcId = getIdCode(QrIdentifier.CRC);
            var crc = qrDataObjects
                .Where(it => it.Id == crcId)
                .LastOrDefault()
                ?.RawValue ?? string.Empty;
            var qry = qrDataObjects
                .Where(it => it.Id != crcId)
                .OrderBy(it => it.Id)
                .Select(it => it.RawValue)
                .Union(new[] { crc });
            return string.Join(string.Empty, qry);
        }
    }
}
