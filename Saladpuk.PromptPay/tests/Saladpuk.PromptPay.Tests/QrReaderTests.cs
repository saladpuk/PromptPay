using Saladpuk.PromptPay.Facades;
using Saladpuk.PromptPay.Models;
using Xunit;

namespace Saladpuk.PromptPay.Tests
{
    public class QrReaderTests
    {
        private PromptPayQrReader sut = new PromptPayQrReader();

        [Fact]
        public void DefaultStaticCreditTransferQRMustBeReadable()
        {
            var qrCode = PPay.StaticQR.CreateCreditTransferQrCode();
            var actual = sut.Read(qrCode);
            var expected = new QrInfo()
                .InitializeDefault()
                .SetPlainCreditTransfer();
            actual.ValidateWith(expected);
        }

        [Fact]
        public void DefaultDynamicCreditTransferQRMustBeReadable()
        {
            var qrCode = PPay.DynamicQR.CreateCreditTransferQrCode();
            var actual = sut.Read(qrCode);
            var expected = new QrInfo()
                .InitializeDefault(staticQr: false)
                .SetPlainCreditTransfer();
            actual.ValidateWith(expected);
        }

        [Fact]
        public void DefaultStaticBillPaymentQRMustBeReadable()
        {
            var qrCode = PPay.StaticQR.GetBillPaymentQrCode();
            var actual = sut.Read(qrCode);
            var expected = new QrInfo()
                .InitializeDefault()
                .SetPlainBillPayment();
            actual.ValidateWith(expected);
        }

        [Fact]
        public void DefaultDynamicBillPaymentQRMustBeReadable()
        {
            var qrCode = PPay.DynamicQR.GetBillPaymentQrCode();
            var actual = sut.Read(qrCode);
            var expected = new QrInfo()
                .InitializeDefault(staticQr: false)
                .SetPlainBillPayment();
            actual.ValidateWith(expected);
        }

        // TODO: Test cases
        // The length of the payload should not exceed 512 alphanumeric characters.
    }
}
