using Saladpuk.PromptPay.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Saladpuk.PromptPay
{
    public class QrBuilder
    {
        private readonly List<QrDataObject> qrDataObjects;

        public QrBuilder()
        {
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

        public QrBuilder SetBillPayment(BillPayment payment)
        {
            var aidRec = createAIDRecord();
            var billerRec = createBillerRecord();
            const string Reference1 = "02";
            var ref1Rec = formatRecord(Reference1, payment.Reference1);
            const string Reference2 = "03";
            var ref2Rec = formatRecord(Reference2, payment.Reference2);

            var value = $"{aidRec}{billerRec}{ref1Rec}{ref2Rec}";
            var digits = value.Length.ToString("00");

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
                const string BillderId = "01";
                return formatRecord(BillderId, $"{payment.NationalIdOrTaxId}{payment.Suffix}");
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
