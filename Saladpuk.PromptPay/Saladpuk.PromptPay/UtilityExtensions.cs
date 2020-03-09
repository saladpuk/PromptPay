using Saladpuk.Contracts;
using Saladpuk.Contracts.EMVCo;
using System;

namespace Saladpuk.PromptPay
{
    public static class UtilityExtensions
    {
        public static string GetCode(this QrIdentifier identifier, string format = "00")
            => ((int)identifier).ToString(format);

        public static string GetCode(this CurrencyCode code, string format = "000")
            => ((int)code).ToString(format);

        public static string GetLength(this string value, string format = "00")
            => value.Length.ToString(format);

        public static string GetCode(this int value, string format = "00")
            => value.ToString(format);

        public static string GetPositiveAmount(this double value, string format = "0.00")
            => Math.Abs(value).ToString(format);
    }
}
