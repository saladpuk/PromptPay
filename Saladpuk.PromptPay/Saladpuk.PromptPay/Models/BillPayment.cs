namespace Saladpuk.PromptPay.Models
{
    public class BillPayment
    {
        public string NationalIdOrTaxId { get; set; }
        public string Suffix { get; set; }
        public string Reference1 { get; set; }
        public string Reference2 { get; set; }
        public bool DomesticMerchant { get; set; }

        public BillPayment()
        {
        }

        public BillPayment(string nationalOrTaxId, string suffix, string ref1 = "", string ref2 = "", bool isDomestic = true)
        {
            NationalIdOrTaxId = nationalOrTaxId;
            Suffix = suffix;
            Reference1 = ref1;
            Reference2 = ref2;
            DomesticMerchant = isDomestic;
        }
    }
}
