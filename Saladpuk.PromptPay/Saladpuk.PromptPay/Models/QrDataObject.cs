using System;
using System.Linq;
using System.Text.Json.Serialization;

namespace Saladpuk.PromptPay.Models
{
    public class QrDataObject
    {
        public string RawValue { get; }

        /// <summary>
        /// The ID is coded as a two-digit numeric value, with a value ranging from "00" to "99".
        /// </summary>
        public string Id => RawValue[0..2];

        /// <summary>
        /// The length is coded as a two-digit numeric value, with a value ranging from "01" to "99".
        /// </summary>
        public string LengthCode => RawValue[2..4];

        /// <summary>
        /// The value field has a minimum length of one character and maximum length of 99 characters.
        /// </summary>
        public string Value => RawValue[4..^0];

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
