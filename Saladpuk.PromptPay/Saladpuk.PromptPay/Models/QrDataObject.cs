namespace Saladpuk.PromptPay.Models
{
    public class QrDataObject
    {
        public string RawValue { get; }
        public string Id => RawValue[0..2];
        public string Length => RawValue[2..4];
        public string Value => RawValue[4..^0];

        public QrDataObject(string rawValue)
            => RawValue = rawValue;
    }
}
