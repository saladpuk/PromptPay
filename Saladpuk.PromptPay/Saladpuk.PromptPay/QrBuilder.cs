using Saladpuk.PromptPay.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Saladpuk.PromptPay
{
    public class QrBuilder
    {
        private readonly List<QrDataObject> qrDataObjects;

        public QrBuilder(string version = "01")
        {
            qrDataObjects = new List<QrDataObject>();
            Add(QrIdentifier.PayloadFormatIndicator, version);
        }

        public QrBuilder Add(QrIdentifier identifier, string data)
        {
            if (string.IsNullOrWhiteSpace(data))
            {
                throw new ArgumentException("Invalid data");
            }

            var id = ((int)identifier).ToString("00");
            var digits = getDigits(data);
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

        private void removeOldRecordIfExists(string id)
        {
            var selected = qrDataObjects.FirstOrDefault(it => it.Id == id);
            if (selected != null)
            {
                qrDataObjects.Remove(selected);
            }
        }

        private string getDigits(string value) => (value ?? string.Empty).Length.ToString("00");

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
