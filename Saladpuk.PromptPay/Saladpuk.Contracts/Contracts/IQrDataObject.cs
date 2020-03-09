using Saladpuk.Contracts.EMVCo;

namespace Saladpuk.Contracts
{
    public interface IQrDataObject
    {
        string RawValue { get; }

        /// <summary>
        /// The ID is coded as a two-digit numeric value, with a value ranging from "00" to "99".
        /// </summary>
        string Id { get; }

        /// <summary>
        /// The length is coded as a two-digit numeric value, with a value ranging from "01" to "99".
        /// </summary>
        string Length { get; }

        /// <summary>
        /// The value field has a minimum length of one character and maximum length of 99 characters.
        /// </summary>
        string Value { get; }

        QrIdentifier Identifier { get; }
    }
}
