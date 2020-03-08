using Saladpuk.PromptPay.Models;
using System.Collections.Generic;

namespace Saladpuk.PromptPay
{
    public class QrReader
    {
        private List<QrDataObject> segments = new List<QrDataObject>();

        public QrInfo ConvertToQrInfo(string qrCode)
        {
            extractText(qrCode);
            return new QrInfo(segments);
        }

        private void extractText(string text)
        {
            var reader = new QrDataObject(text);
            var (targetSegment, other) = extractSegments(reader);
            segments.Add(targetSegment);
            if (!string.IsNullOrWhiteSpace(other))
            {
                extractText(other);
            }
        }
        private (QrDataObject, string) extractSegments(QrDataObject data)
        {
            var currentValue = $"{data.Id}{data.LengthCode}{data.Value[0..data.Length]}";
            var firstSegment = new QrDataObject(currentValue);
            var other = data.RawValue[currentValue.Length..^0];
            return (firstSegment, other);
        }
    }
}
