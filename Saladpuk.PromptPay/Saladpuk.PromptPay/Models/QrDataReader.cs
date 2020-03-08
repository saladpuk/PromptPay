//using System;
//using System.Linq;

//namespace Saladpuk.PromptPay.Models
//{
//    public class QrDataReader
//    {
//        private readonly QrDataObject data;

//        public string Id => data.Id;

//        public int Length
//        {
//            get
//            {
//                if (!int.TryParse(data.LengthCode, out int length))
//                {
//                    throw new ArgumentException("QR identifier code isn't valid.");
//                }
//                return length;
//            }
//        }

//        public string Value => data.Value;

//        public QrIdentifier Identifier
//        {
//            get
//            {
//                if (!Enum.TryParse(Id, out QrIdentifier identifier))
//                {
//                    return QrIdentifier.Unknow;
//                }

//                var merchantIdentifierRange = Enumerable.Range(2, 50);
//                var isMerchant = merchantIdentifierRange.Contains((int)identifier);
//                return isMerchant ? QrIdentifier.MerchantAccountInformation : identifier;
//            }
//        }

//        public QrDataReader(string text)
//        {
//            data = new QrDataObject(text);
//        }
//    }
//}
