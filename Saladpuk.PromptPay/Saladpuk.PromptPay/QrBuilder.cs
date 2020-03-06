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
            const string Reusable = "12";
            Add(QrIdentifier.PointOfInitiationMethod, Reusable);
            return this;
        }

        public QrBuilder SetDynamicQR()
        {
            const string OneTimeOnly = "11";
            Add(QrIdentifier.PointOfInitiationMethod, OneTimeOnly);
            return this;
        }

        public QrBuilder SetTransactionAmount(double amount)
        {
            var value = Math.Abs(amount).ToString("0.00");
            Add(QrIdentifier.TransactionAmount, value);
            return this;
        }

        public QrBuilder SetCountryCode(string code)
        {
            var region = new System.Globalization.RegionInfo(code);
            Add(QrIdentifier.CountryCode, region.TwoLetterISORegionName);
            return this;
        }

        public QrBuilder SetCurrencyCode(CurrencyCode code)
        {
            var value = ((int)code).ToString();
            Add(QrIdentifier.TransactionCurrency, value);
            return this;
        }

        public QrBuilder SetCyclicRedundancyCheck(int digits = 4)
        {
            var value = digits.ToString("00");
            Add(QrIdentifier.CRC, value);
            return this;
        }

        private void removeOldRecordIfExists(string id)
        {
            var selected = qrDataObjects.FirstOrDefault(it => it.Id == id);
            if (selected != null)
            {
                qrDataObjects.Remove(selected);
            }
        }

        public string GetQrCode(ICyclicRedundancyCheck crc)
        {
            var code = ToString();
            var checksum = crc.ComputeChecksum(code);
            return $"{code}{checksum}";
        }

        public override string ToString()
        {
            var qry = qrDataObjects.OrderBy(it => it.Id).Select(it => it.RawValue);
            return string.Join(string.Empty, qry);
        }
    }
}
