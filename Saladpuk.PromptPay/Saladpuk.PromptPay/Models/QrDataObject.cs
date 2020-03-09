using System;
using System.Linq;
using Newtonsoft.Json;
using Saladpuk.Contracts.EMVCo;

namespace Saladpuk.PromptPay.Models
{
    public class QrDataObject
    {
        private string id;
        private string lengthCode;
        private string value;

        public string RawValue { get; }

        /// <summary>
        /// The ID is coded as a two-digit numeric value, with a value ranging from "00" to "99".
        /// </summary>
        public string Id
        {
            get
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    const int IdIndex = 0;
                    const int ContentLength = 2;
                    id = RawValue.Substring(IdIndex, ContentLength);
                }
                return id;
            }
        }

        /// <summary>
        /// The length is coded as a two-digit numeric value, with a value ranging from "01" to "99".
        /// </summary>
        public string LengthCode
        {
            get
            {
                if (string.IsNullOrWhiteSpace(lengthCode))
                {
                    const int LengthIndex = 2;
                    const int ContentLength = 2;
                    lengthCode = RawValue.Substring(LengthIndex, ContentLength);
                }
                return lengthCode;
            }
        }

        /// <summary>
        /// The value field has a minimum length of one character and maximum length of 99 characters.
        /// </summary>
        public string Value
        {
            get
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    const int ValueIndex = 4;
                    value = RawValue.Substring(ValueIndex);
                }
                return value;
            }
        }

        [JsonIgnore]
        public int Length
        {
            get
            {
                if (!int.TryParse(LengthCode, out int length))
                {
                    throw new ArgumentException("QR identifier code isn't valid.");
                }
                return length;
            }
        }

        [JsonIgnore]
        public QrIdentifier Identifier
        {
            get
            {
                if (!Enum.TryParse(Id, out QrIdentifier identifier))
                {
                    return QrIdentifier.Unknow;
                }

                var merchantIdentifierRange = Enumerable.Range(2, 50);
                var isMerchant = merchantIdentifierRange.Contains((int)identifier);
                return isMerchant ? QrIdentifier.MerchantAccountInformation : identifier;
            }
        }

        public QrDataObject(string rawValue)
            => RawValue = rawValue;
    }
}
