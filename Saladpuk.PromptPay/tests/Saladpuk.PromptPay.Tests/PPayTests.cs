using FluentAssertions;
using Saladpuk.PromptPay.Facades;
using Xunit;

namespace Saladpuk.PromptPay.Tests
{
    public class PPayTests
    {
        const string EMVCoVersion = "000201";
        const string Static = "010212";
        const string Dynamic = "010211";
        const string Country = "5802TH";
        const string Currency = "5303764";
        const string CRC16Prefix = "6304";

        const string MerchantPresented = "A000000677010111";
        const string CustomerPresented = "A000000677010114";

        const string DomesticMerchant = "A000000677010112";
        const string CrossBorderMerchant = "A000000677012006";

        [Fact]
        public void SimpleStaticCreditTransferQRWithMerchantPresentedShouldInTheRightFormat()
        {
            var actual = PPay.StaticQR.MerchantPresentedQR().GetCreditTransferQR();
            var Merchant = $"29200016{MerchantPresented}";
            var expected = $"{EMVCoVersion}{Static}{Merchant}{Currency}{Country}{CRC16Prefix}";
            actual.Should().NotBeNull().And.StartWith(expected);
        }
        [Fact]
        public void SimpleStaticCreditTransferQRWithCustomerPresentedShouldInTheRightFormat()
        {
            var actual = PPay.StaticQR.CustomerPresentedQR().GetCreditTransferQR();
            var Merchant = $"29200016{CustomerPresented}";
            var expected = $"{EMVCoVersion}{Static}{Merchant}{Currency}{Country}{CRC16Prefix}";
            actual.Should().NotBeNull().And.StartWith(expected);
        }
        [Fact]
        public void SimpleDynamicCreditTransferQRWithMerchantPresentedShouldInTheRightFormat()
        {
            var actual = PPay.DynamicQR.GetCreditTransferQR();
            var Merchant = $"29200016{MerchantPresented}";
            var expected = $"{EMVCoVersion}{Dynamic}{Merchant}{Currency}{Country}{CRC16Prefix}";
            actual.Should().NotBeNull().And.StartWith(expected);
        }
        [Fact]
        public void SimpleDynamicCreditTransferQRWithCustomerPresentedShouldInTheRightFormat()
        {
            var actual = PPay.DynamicQR.CustomerPresentedQR().GetCreditTransferQR();
            var Merchant = $"29200016{CustomerPresented}";
            var expected = $"{EMVCoVersion}{Dynamic}{Merchant}{Currency}{Country}{CRC16Prefix}";
            actual.Should().NotBeNull().And.StartWith(expected);
        }

        [Fact]
        public void SimpleStaticBillPaymentQRWithDomesticMerchantShouldInTheRightFormat()
        {
            var actual = PPay.StaticQR.DomesticMerchant().GetBillPaymentQR();
            var Merchant = $"30200016{DomesticMerchant}";
            var expected = $"{EMVCoVersion}{Static}{Merchant}{Currency}{Country}{CRC16Prefix}";
            actual.Should().NotBeNull().And.StartWith(expected);
        }
        [Fact]
        public void SimpleStaticBillPaymentQRWithCrosBorderMerchantShouldInTheRightFormat()
        {
            var actual = PPay.StaticQR.CrossBorderMerchant().GetBillPaymentQR();
            var Merchant = $"30200016{CrossBorderMerchant}";
            var expected = $"{EMVCoVersion}{Static}{Merchant}{Currency}{Country}{CRC16Prefix}";
            actual.Should().NotBeNull().And.StartWith(expected);
        }
        [Fact]
        public void SimpleDynamicBillPaymentQRWithDomesticMerchantShouldInTheRightFormat()
        {
            var actual = PPay.DynamicQR.DomesticMerchant().GetBillPaymentQR();
            var Merchant = $"30200016{DomesticMerchant}";
            var expected = $"{EMVCoVersion}{Dynamic}{Merchant}{Currency}{Country}{CRC16Prefix}";
            actual.Should().NotBeNull().And.StartWith(expected);
        }
        [Fact]
        public void SimpleDynamicBillPaymentQRWithCrosBorderMerchantShouldInTheRightFormat()
        {
            var actual = PPay.DynamicQR.CrossBorderMerchant().GetBillPaymentQR();
            var Merchant = $"30200016{CrossBorderMerchant}";
            var expected = $"{EMVCoVersion}{Dynamic}{Merchant}{Currency}{Country}{CRC16Prefix}";
            actual.Should().NotBeNull().And.StartWith(expected);
        }
    }
}
