using FluentAssertions;
using Saladpuk.Contracts.EMVCo;
using Xunit;

namespace Saladpuk.PromptPay.Tests
{
    public class QrBuilderTests
    {
        private QrBuilder sut = new QrBuilder();

        [Fact]
        public void DefaultQrBuilderMustBeEmpty()
            => sut.ToString().Should().BeEquivalentTo(string.Empty);

        [Theory]
        [InlineData(QrIdentifier.PayloadFormatIndicator, "01", "000201")]
        [InlineData(QrIdentifier.PayloadFormatIndicator, "02", "000202")]
        public void SimpleGeneration(QrIdentifier id, string value, string expected)
            => sut.Add(id, value).ToString().Should().BeEquivalentTo(expected);

        [Theory]
        [InlineData(0, "54040.00")]
        [InlineData(50, "540550.00")]
        [InlineData(50.00, "540550.00")]
        [InlineData(101.001, "5406101.00")]
        [InlineData(102.004, "5406102.00")]
        [InlineData(201.005, "5406201.01")]
        [InlineData(202.0054, "5406202.01")]
        [InlineData(203.0055, "5406203.01")]
        public void AddTransactionAmount(double amount, string expected)
            => sut.SetTransactionAmount(amount).ToString().Should().BeEquivalentTo(expected);

        [Theory]
        [InlineData(-50, "540550.00")]
        public void AddTransactionAmountWithNegativeValue(double amount, string expected)
            => sut.SetTransactionAmount(amount).ToString().Should().BeEquivalentTo(expected);

        // TODO: Test cases
        // The Payload Format Indicator (ID "00") shall be the first data object in the QR Code.
        // The CRC (ID "63") shall be the last data object in the QR Code. (All other data objects under the root may be placed at any position.
    }
}
