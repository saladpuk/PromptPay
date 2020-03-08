using FluentAssertions;
using Saladpuk.PromptPay.Facades;
using Xunit;
using ppay = Saladpuk.PromptPay.PromptPayValues;

namespace Saladpuk.PromptPay.Tests
{
    public class PPayTests
    {
        private const string EMVCoVersion = "000201";
        private const string Static = "010212";
        private const string Dynamic = "010211";
        private const string Country = "5802TH";
        private const string Currency = "5303764";
        private const string CRC16Prefix = "6304";
        private const int CheckSumDigits = 4;

        #region Default

        [Fact]
        public void DefaultStaticCreditTransferQRMustBeMerchantPresentedQRFormat()
        {
            var actual = PPay.StaticQR.GetCreditTransferQR();
            var Merchant = $"29200016{ppay.MerchantPresented}";
            var expected = $"{EMVCoVersion}{Static}{Merchant}{Currency}{Country}{CRC16Prefix}";
            validateQRFormat(actual, expected);
        }

        [Fact]
        public void DefaultDynamicCreditTransferQRMustBeMerchantPresentedQRFormat()
        {
            var actual = PPay.DynamicQR.GetCreditTransferQR();
            var Merchant = $"29200016{ppay.MerchantPresented}";
            var expected = $"{EMVCoVersion}{Dynamic}{Merchant}{Currency}{Country}{CRC16Prefix}";
            validateQRFormat(actual, expected);
        }

        [Fact]
        public void DefaultStaticBillPaymentQRMustBeDomesticMerchantFormat()
        {
            var actual = PPay.StaticQR.GetBillPaymentQR();
            var Merchant = $"30200016{ppay.DomesticMerchant}";
            var expected = $"{EMVCoVersion}{Static}{Merchant}{Currency}{Country}{CRC16Prefix}";
            validateQRFormat(actual, expected);
        }

        [Fact]
        public void DefaultDynamicBillPaymentQRMustBeDomesticMerchantFormat()
        {
            var actual = PPay.DynamicQR.GetBillPaymentQR();
            var Merchant = $"30200016{ppay.DomesticMerchant}";
            var expected = $"{EMVCoVersion}{Dynamic}{Merchant}{Currency}{Country}{CRC16Prefix}";
            validateQRFormat(actual, expected);
        }

        #endregion Default

        #region Static QR

        [Fact]
        public void SimpleStaticCreditTransferQRWithMerchantPresentedShouldInTheRightFormat()
        {
            var actual = PPay.StaticQR.MerchantPresentedQR().GetCreditTransferQR();
            var Merchant = $"29200016{ppay.MerchantPresented}";
            var expected = $"{EMVCoVersion}{Static}{Merchant}{Currency}{Country}{CRC16Prefix}";
            validateQRFormat(actual, expected);
        }

        [Fact]
        public void SimpleStaticCreditTransferQRWithCustomerPresentedShouldInTheRightFormat()
        {
            var actual = PPay.StaticQR.CustomerPresentedQR().GetCreditTransferQR();
            var Merchant = $"29200016{ppay.CustomerPresented}";
            var expected = $"{EMVCoVersion}{Static}{Merchant}{Currency}{Country}{CRC16Prefix}";
            validateQRFormat(actual, expected);
        }

        [Fact]
        public void SimpleDynamicCreditTransferQRWithMerchantPresentedShouldInTheRightFormat()
        {
            var actual = PPay.DynamicQR.GetCreditTransferQR();
            var Merchant = $"29200016{ppay.MerchantPresented}";
            var expected = $"{EMVCoVersion}{Dynamic}{Merchant}{Currency}{Country}{CRC16Prefix}";
            validateQRFormat(actual, expected);
        }

        [Fact]
        public void SimpleDynamicCreditTransferQRWithCustomerPresentedShouldInTheRightFormat()
        {
            var actual = PPay.DynamicQR.CustomerPresentedQR().GetCreditTransferQR();
            var Merchant = $"29200016{ppay.CustomerPresented}";
            var expected = $"{EMVCoVersion}{Dynamic}{Merchant}{Currency}{Country}{CRC16Prefix}";
            validateQRFormat(actual, expected);
        }

        #endregion Static QR

        #region Dynamic QR

        [Fact]
        public void SimpleStaticBillPaymentQRWithDomesticMerchantShouldInTheRightFormat()
        {
            var actual = PPay.StaticQR.DomesticMerchant().GetBillPaymentQR();
            var Merchant = $"30200016{ppay.DomesticMerchant}";
            var expected = $"{EMVCoVersion}{Static}{Merchant}{Currency}{Country}{CRC16Prefix}";
            validateQRFormat(actual, expected);
        }

        [Fact]
        public void SimpleStaticBillPaymentQRWithCrosBorderMerchantShouldInTheRightFormat()
        {
            var actual = PPay.StaticQR.CrossBorderMerchant().GetBillPaymentQR();
            var Merchant = $"30200016{ppay.CrossBorderMerchant}";
            var expected = $"{EMVCoVersion}{Static}{Merchant}{Currency}{Country}{CRC16Prefix}";
            validateQRFormat(actual, expected);
        }

        [Fact]
        public void SimpleDynamicBillPaymentQRWithDomesticMerchantShouldInTheRightFormat()
        {
            var actual = PPay.DynamicQR.DomesticMerchant().GetBillPaymentQR();
            var Merchant = $"30200016{ppay.DomesticMerchant}";
            var expected = $"{EMVCoVersion}{Dynamic}{Merchant}{Currency}{Country}{CRC16Prefix}";
            validateQRFormat(actual, expected);
        }

        [Fact]
        public void SimpleDynamicBillPaymentQRWithCrosBorderMerchantShouldInTheRightFormat()
        {
            var actual = PPay.DynamicQR.CrossBorderMerchant().GetBillPaymentQR();
            var Merchant = $"30200016{ppay.CrossBorderMerchant}";
            var expected = $"{EMVCoVersion}{Dynamic}{Merchant}{Currency}{Country}{CRC16Prefix}";
            validateQRFormat(actual, expected);
        }

        #endregion Dynamic QR

        private static void validateQRFormat(string actual, string expected)
            => actual.Should().NotBeNull().And.StartWith(expected).And.HaveLength(expected.Length + CheckSumDigits);
    }
}
