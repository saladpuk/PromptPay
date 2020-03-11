using Saladpuk.EMVCo.Contracts;
using Saladpuk.EMVCo.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using emv = Saladpuk.EMVCo.Contracts.EMVCoCodeConventions;

namespace Saladpuk.EMVCo
{
    /// <summary>
    /// ตัวสร้าง QR ตามมาตรฐาน EMVCo
    /// </summary>
    /// <typeparam name="T">ตัวสร้าง QR</typeparam>
    public class QrBuilder : IEMVCoBuilder<QrBuilder>
    {
        #region Properties

        /// <summary>
        /// ส่วนประกอบทั้งหมดของ QR code
        /// </summary>
        public List<IQrDataObject> QrDataObjects { get; }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// กำหนดค่าเริ่มต้นให้กับ ตัวสร้าง QR
        /// </summary>
        public QrBuilder()
        {
            QrDataObjects = new List<IQrDataObject>();
        }

        #endregion Constructors

        #region IEMVCo

        public QrBuilder Add(QrIdentifier identifier, string data)
        {
            if (string.IsNullOrWhiteSpace(data))
            {
                throw new ArgumentException("Invalid data");
            }

            var id = getIdCode(identifier);
            var digits = getLengthDigits(data ?? string.Empty);
            removeOldRecordIfExists(id);
            QrDataObjects.Add(new QrDataObject($"{id}{digits}{data}"));
            return this;
        }

        public QrBuilder Add(string rawData)
        {
            var isArgumentValid = !string.IsNullOrWhiteSpace(rawData)
                && rawData.Length >= emv.MinSegmentLength;
            if (!isArgumentValid)
            {
                throw new ArgumentException("Invalid data");
            }

            var data = new QrDataObject(rawData);
            removeOldRecordIfExists(data.Id);
            QrDataObjects.Add(data);
            return this;
        }

        public QrBuilder SetStaticQR()
            => Add(QrIdentifier.PointOfInitiationMethod, emv.Static);

        public QrBuilder SetDynamicQR()
            => Add(QrIdentifier.PointOfInitiationMethod, emv.Dynamic);

        public QrBuilder SetTransactionAmount(double amount)
            => Add(QrIdentifier.TransactionAmount, Math.Abs(amount).ToString("0.00"));

        public QrBuilder SetCountryCode(string code)
            => Add(QrIdentifier.CountryCode, new RegionInfo(code).TwoLetterISORegionName);

        public QrBuilder SetCurrencyCode(CurrencyCode code)
            => Add(QrIdentifier.TransactionCurrency, ((int)code).ToString("000"));

        public QrBuilder SetPayloadFormatIndicator(string version = "01")
            => Add(QrIdentifier.PayloadFormatIndicator, version);

        public QrBuilder SetCyclicRedundancyCheck(ICyclicRedundancyCheck crc)
        {
            var id = getIdCode(QrIdentifier.CRC);
            var currentCode = $"{ToString()}{id}{emv.CRCDigits.ToString("00")}";
            var computedValue = crc.ComputeChecksum(currentCode);
            Add(QrIdentifier.CRC, computedValue);
            return this;
        }

        #endregion IEMVCo

        #region Methods

        /// <summary>
        /// เรียกดูโค้ดทั้งหมดโดยไม่ระบุประเภทการใช้งาน
        /// </summary>
        public override string ToString()
        {
            var crcId = getIdCode(QrIdentifier.CRC);
            var crc = QrDataObjects
                .Where(it => it.Id == crcId)
                .LastOrDefault()
                ?.RawValue ?? string.Empty;
            var qry = QrDataObjects
                .Where(it => it.Id != crcId)
                .OrderBy(it => it.Id)
                .Select(it => it.RawValue)
                .Union(new[] { crc });
            return string.Join(string.Empty, qry);
        }

        private void removeOldRecordIfExists(string id)
        {
            var selected = QrDataObjects.FirstOrDefault(it => it.Id == id);
            if (selected != null)
            {
                QrDataObjects.Remove(selected);
            }
        }

        private string getLengthDigits(string value)
            => value.Length.ToString("00");

        private string getIdCode(QrIdentifier identifier)
            => ((int)identifier).ToString("00");

        #endregion Methods
    }
}
