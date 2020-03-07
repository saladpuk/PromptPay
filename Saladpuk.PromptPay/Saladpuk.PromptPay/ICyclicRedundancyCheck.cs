namespace Saladpuk.PromptPay
{
    /// <summary>
    /// Checksum calculated over all the data objects included in the QR Code.
    /// </summary>
    /// <remarks>
    /// The checksum shall be calculated according to[ISO / IEC 13239] using the
    /// polynomial '1021' (hex) and initial value 'FFFF' (hex). The data over which the
    /// checksum is calculated shall cover all data objects, including their ID, Length and
    /// Value, to be included in the QR Code, in their respective order, as well as the ID
    /// and Length of the CRC itself(but excluding its Value).
    /// Following the calculation of the checksum, the resulting 2-byte hexadecimal value
    /// shall be encoded as a 4-character Alphanumeric Special value by converting each
    /// nibble to an Alphanumeric Special character.
    /// </remarks>
    /// <example>
    /// A CRC with a two-byte hexadecimal value of '007B' is included in the
    /// QR Code as "6304007B"
    /// </example>
    public interface ICyclicRedundancyCheck
    {
        string ComputeChecksum(string data);
    }
}
