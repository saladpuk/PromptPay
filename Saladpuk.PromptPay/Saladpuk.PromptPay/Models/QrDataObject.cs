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
        public string Length => RawValue[2..4];

        /// <summary>
        /// The value field has a minimum length of one character and maximum length of 99 characters.
        /// </summary>
        public string Value => RawValue[4..^0];

        public QrDataObject(string rawValue)
            => RawValue = rawValue;
    }
}
