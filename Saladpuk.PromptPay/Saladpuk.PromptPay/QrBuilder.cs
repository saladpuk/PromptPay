using Saladpuk.PromptPay.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using emv = Saladpuk.PromptPay.EMVCoValues;
using ppay = Saladpuk.PromptPay.PromptPayValues;

namespace Saladpuk.PromptPay
{
    public class QrBuilder
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

        public QrBuilder Add(QrIdentifier identifier, string data)
        {
            if (string.IsNullOrWhiteSpace(data))
            {
                throw new ArgumentException("Invalid data");
            }

            var id = identifier.GetCode();
            var digits = (data ?? string.Empty).GetLength();
            removeOldRecordIfExists(id);
            qrDataObjects.Add(new QrDataObject($"{id}{digits}{data}"));
            return this;
        }

        public QrBuilder Add(string rawData)
        {
            var isArgumentValid = !string.IsNullOrWhiteSpace(rawData)
                && rawData.Length >= emv.MinContentLength;
            if (!isArgumentValid)
            {
                throw new ArgumentException("Invalid data");
            }

            var id = rawData[0..2];
            removeOldRecordIfExists(id);
            qrDataObjects.Add(new QrDataObject(rawData));
            return this;
        }

        public QrBuilder SetStaticQR()
            => Add(QrIdentifier.PointOfInitiationMethod, emv.Static);

        public QrBuilder SetDynamicQR()
            => Add(QrIdentifier.PointOfInitiationMethod, emv.Dynamic);

        public QrBuilder SetTransactionAmount(double amount)
            => Add(QrIdentifier.TransactionAmount, amount.GetPositiveAmount());

        public QrBuilder SetCountryCode(string code)
            => Add(QrIdentifier.CountryCode, new RegionInfo(code).TwoLetterISORegionName);

        public QrBuilder SetCurrencyCode(CurrencyCode code)
            => Add(QrIdentifier.TransactionCurrency, code.GetCode());

        public QrBuilder SetPayloadFormatIndicator(string version = "01")
            => Add(QrIdentifier.PayloadFormatIndicator, version);

        public QrBuilder SetCyclicRedundancyCheck(ICyclicRedundancyCheck crc)
        {
            var id = QrIdentifier.CRC.GetCode();
            var currentCode = $"{ToString()}{id}{emv.CRCDigits.GetCode()}";
            var computedValue = crc.ComputeChecksum(currentCode);
            Add(QrIdentifier.CRC, computedValue);
            return this;
        }

        public QrBuilder SetCreditTransfer(CreditTransfer transfer)
        {
            creditTransfer = transfer;
            billPayment.NationalIdOrTaxId = creditTransfer.NationalIdOrTaxId;

            var value = getValue();
            var digits = value.GetLength();
            removeMerchantRecordsIfExists();
            Add($"{ppay.CreditTransferTagId}{digits}{value}");
            return this;

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
                    mobileNo = $"{Prefix}{mobileNo.Substring(1, mobileNo.Length - 1)}";
                }
                return formatRecord(ppay.MobileId, mobileNo);
            }
        }

        public QrBuilder SetBillPayment(BillPayment payment)
        {
            billPayment = payment;
            creditTransfer.NationalIdOrTaxId = payment.NationalIdOrTaxId;

            var value = getValue();
            var digits = value.GetLength();
            removeMerchantRecordsIfExists();
            Add($"{ppay.BillPaymentTagId}{digits}{value}");
            return this;

            string getValue()
            {
                payment.Suffix ??= "00";
                var aidRec = formatRecord(ppay.AID, payment.DomesticMerchant ? ppay.DomesticMerchant : ppay.CrossBorderMerchant);
                var billerRec = string.IsNullOrWhiteSpace(payment.NationalIdOrTaxId) ? string.Empty : formatRecord(ppay.BillderId, $"{payment.NationalIdOrTaxId}{payment.Suffix}");
                var ref1Rec = string.IsNullOrWhiteSpace(payment.Reference1) ? string.Empty : formatRecord(ppay.Reference1, payment.Reference1);
                var ref2Rec = string.IsNullOrWhiteSpace(payment.Reference2) ? string.Empty : formatRecord(ppay.Reference2, payment.Reference2);
                return $"{aidRec}{billerRec}{ref1Rec}{ref2Rec}";
            }
        }

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
            => $"{id}{value.GetLength()}{value}";

        private void removeOldRecordIfExists(string id)
        {
            var selected = qrDataObjects.FirstOrDefault(it => it.Id == id);
            if (selected != null)
            {
                qrDataObjects.Remove(selected);
            }
        }

        public override string ToString()
        {
            var crcId = QrIdentifier.CRC.GetCode();
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
