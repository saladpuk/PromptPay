using Newtonsoft.Json;
using Saladpuk.Contracts;
using Saladpuk.Contracts.EMVCo;
using System;
using System.Linq;
using emv = Saladpuk.Contracts.EMVCo.EMVCoCodeConventions;

namespace Saladpuk.PromptPay.Models
{
    public class QrDataObject : IQrDataObject
    {
        public string RawValue { get; }
        public string Id { get; }
        public string Length { get; }
        public string Value { get; }

        [JsonIgnore]
        public QrIdentifier IdByConvention
        {
            get
            {
                if (!Enum.TryParse(Id, out QrIdentifier identifier))
                {
                    throw new ArgumentOutOfRangeException("QR identifier code isn't valid.");
                }

                var id = (int)identifier;
                var isMerchant = emv.MerchantIdRange.Contains(id);
                if (isMerchant)
                {
                    return QrIdentifier.MerchantAccountInformation;
                }

                var isReserved = emv.RFUIdRange.Contains(id);
                if (isReserved)
                {
                    return QrIdentifier.RFU;
                }

                return identifier;
            }
        }

        public QrDataObject(string rawValue)
        {
            var isArgumentValid = !string.IsNullOrWhiteSpace(rawValue)
                && rawValue.Length >= emv.MinSegmentLength;
            if (!isArgumentValid)
            {
                throw new ArgumentException("Content must has a minimum length of 5 characters.");
            }

            RawValue = rawValue;
            Id = getIdSegment();
            Length = getLength();
            Value = getValue();

            string getIdSegment()
            {
                const int IdIndex = 0;
                const int ContentLength = 2;
                return RawValue.Substring(IdIndex, ContentLength);
            }
            string getLength()
            {
                const int LengthIndex = 2;
                const int ContentLength = 2;
                return RawValue.Substring(LengthIndex, ContentLength);
            }
            string getValue()
            {
                const int ValueIndex = 4;
                return RawValue.Substring(ValueIndex);
            }
        }

    }
}
