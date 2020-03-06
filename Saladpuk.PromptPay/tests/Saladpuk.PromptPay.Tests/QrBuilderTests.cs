using FluentAssertions;
using Xunit;

namespace Saladpuk.PromptPay.Tests
{
    public class QrBuilderTests
    {
        private QrBuilder sut = new QrBuilder();

        [Fact]
        public void QrWithDefaultVersion()
            => sut.ToString().Should().BeEquivalentTo("000201");

        [Theory]
        [InlineData(QrIdentifier.PayloadFormatIndicator, "01", "000201")]
        [InlineData(QrIdentifier.PayloadFormatIndicator, "02", "000202")]
        public void SimpleGeneration(QrIdentifier id, string value, string expected)
            => sut.Add(id, value).ToString().Should().BeEquivalentTo(expected);
    }
}
