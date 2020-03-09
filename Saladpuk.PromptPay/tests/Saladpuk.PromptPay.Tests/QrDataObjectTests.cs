using FluentAssertions;
using Saladpuk.PromptPay.Models;
using Xunit;

namespace Saladpuk.PromptPay.Tests
{
    public class QrDataObjectTests
    {
        [Theory]
        [InlineData("AABBC", "AA", "BB", "C")]
        [InlineData("5802TH", "58", "02", "TH")]
        public void CorrectQrDataObject(string rawValue, string expectedId, string expectedLength, string expectedValue)
        {
            var sut = new QrDataObject(rawValue);
            sut.RawValue.Should().BeEquivalentTo(rawValue);
            sut.Id.Should().BeEquivalentTo(expectedId);
            sut.Length.Should().BeEquivalentTo(expectedLength);
            sut.Value.Should().BeEquivalentTo(expectedValue);
        }

        // TODO: Test cases
        // Invalid Id.
        // Invalid length.
        // Invalid value's length & format.
    }
}
