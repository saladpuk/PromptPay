using Saladpuk.PromptPay.Models;
using System;
using System.Collections.Generic;
using System.Linq;

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

            var id = ((int)identifier).ToString("00");
            var digits = (data ?? string.Empty).Length.ToString("00");
            removeOldRecordIfExists(id);
            qrDataObjects.Add(new QrDataObject($"{id}{digits}{data}"));
            return this;
        }

        public QrBuilder Add(string rawData)
        {
            const int MinimumLength = 5;
            var isArgumentValid = !string.IsNullOrWhiteSpace(rawData)
                && rawData.Length >= MinimumLength;
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
        {
            const string ReusableCode = "12";
            return Add(QrIdentifier.PointOfInitiationMethod, ReusableCode);
        }

        public QrBuilder SetDynamicQR()
        {
            const string OneTimeOnlyCode = "11";
            return Add(QrIdentifier.PointOfInitiationMethod, OneTimeOnlyCode);
        }

        public QrBuilder SetTransactionAmount(double amount)
        {
            var value = Math.Abs(amount).ToString("0.00");
            return Add(QrIdentifier.TransactionAmount, value);
        }

        public QrBuilder SetCountryCode(string code)
        {
            var region = new System.Globalization.RegionInfo(code);
            return Add(QrIdentifier.CountryCode, region.TwoLetterISORegionName);
        }

        public QrBuilder SetCurrencyCode(CurrencyCode code)
        {
            var value = ((int)code).ToString();
            return Add(QrIdentifier.TransactionCurrency, value);
        }

        public QrBuilder SetPayloadFormatIndicator(string version = "01")
            => Add(QrIdentifier.PayloadFormatIndicator, version);

        public QrBuilder SetCyclicRedundancyCheck(ICyclicRedundancyCheck crc)
        {
            var id = ((int)QrIdentifier.CRC).ToString("00");
            const string DefaultChecksumDigits = "04";
            var currentCode = $"{ToString()}{id}{DefaultChecksumDigits}";
            var crcValue = crc.ComputeChecksum(currentCode);
            Add(QrIdentifier.CRC, crcValue);
            return this;
        }

        public QrBuilder SetCreditTransfer(CreditTransfer transfer)
        {
            creditTransfer = transfer;
            billPayment.NationalIdOrTaxId = creditTransfer.NationalIdOrTaxId;

            var aidRec = createAIDRecord();
            var mobileRec = createMobileRecord();
            var receiverRec = createNationalOrTaxRecord();
            var walletRec = createWalletRecord();
            var bankAccRed = createBankAccountRecord();
            var otaRec = createOTARecord();

            var value = $"{aidRec}{mobileRec}{receiverRec}{walletRec}{bankAccRed}{otaRec}";
            var digits = value.Length.ToString("00");

            removeMerchantRecordsIfExists();
            const string CreditTransferId = "29";
            Add($"{CreditTransferId}{digits}{value}");
            return this;

            string createAIDRecord()
            {
                const string AID = "00";
                const string MerchantPresentedQR = "A000000677010111";
                const string CustomerPresentedQR = "A000000677010114";
                var value = transfer.MerchantPresentedQR ? MerchantPresentedQR : CustomerPresentedQR;
                return formatRecord(AID, value);
            }
            string createMobileRecord()
            {
                if (string.IsNullOrWhiteSpace(transfer.MobileNumber))
                {
                    return string.Empty;
                }
                const string MobileId = "01";
                const string Prefix = "0066";
                var mobileNo = transfer.MobileNumber;
                if (!mobileNo.StartsWith(Prefix))
                {
                    // HACK: Stupid change prefix (refactor this later)
                    mobileNo = $"{Prefix}{mobileNo.Substring(1, mobileNo.Length - 1)}";
                }
                return formatRecord(MobileId, mobileNo);
            }
            string createNationalOrTaxRecord()
            {
                if (string.IsNullOrWhiteSpace(transfer.NationalIdOrTaxId))
                {
                    return string.Empty;
                }
                const string NationalOrTaxId = "02";
                return formatRecord(NationalOrTaxId, transfer.NationalIdOrTaxId);
            }
            string createWalletRecord()
            {
                if (string.IsNullOrWhiteSpace(transfer.EWalletId))
                {
                    return string.Empty;
                }
                const string EWalletId = "03";
                return formatRecord(EWalletId, transfer.EWalletId);
            }
            string createBankAccountRecord()
            {
                if (string.IsNullOrWhiteSpace(transfer.BankAccount))
                {
                    return string.Empty;
                }
                const string BankAccountId = "04";
                return formatRecord(BankAccountId, transfer.BankAccount);
            }
            string createOTARecord()
            {
                if (string.IsNullOrWhiteSpace(transfer.OTA))
                {
                    return string.Empty;
                }
                const string OTAId = "05";
                return formatRecord(OTAId, transfer.OTA);
            }
        }

        public QrBuilder SetBillPayment(BillPayment payment)
        {
            billPayment = payment;
            creditTransfer.NationalIdOrTaxId = payment.NationalIdOrTaxId;

            var aidRec = createAIDRecord();
            var billerRec = createBillerRecord();
            var ref1Rec = string.Empty;
            if (!string.IsNullOrWhiteSpace(payment.Reference1))
            {
                const string Reference1 = "02";
                ref1Rec = formatRecord(Reference1, payment.Reference1);
            }
            var ref2Rec = string.Empty;
            if (!string.IsNullOrWhiteSpace(payment.Reference2))
            {
                const string Reference2 = "03";
                ref2Rec = formatRecord(Reference2, payment.Reference2);
            }

            var value = $"{aidRec}{billerRec}{ref1Rec}{ref2Rec}";
            var digits = value.Length.ToString("00");

            removeMerchantRecordsIfExists();
            const string BillPaymentId = "30";
            Add($"{BillPaymentId}{digits}{value}");
            return this;

            string createAIDRecord()
            {
                const string AID = "00";
                const string DomesticMerchant = "A000000677010112";
                const string CrossBorderMerchant = "A000000677012006";
                var value = payment.DomesticMerchant ? DomesticMerchant : CrossBorderMerchant;
                return formatRecord(AID, value);
            }
            string createBillerRecord()
            {
                if (string.IsNullOrWhiteSpace(payment.NationalIdOrTaxId))
                {
                    return string.Empty;
                }

                payment.Suffix ??= "00";

                const string BillderId = "01";
                return formatRecord(BillderId, $"{payment.NationalIdOrTaxId}{payment.Suffix}");
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
            => $"{id}{value.Length.ToString("00")}{value}";

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
            var crcId = ((int)QrIdentifier.CRC).ToString();
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
