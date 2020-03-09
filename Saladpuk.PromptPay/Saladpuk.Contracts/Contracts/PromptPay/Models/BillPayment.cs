namespace Saladpuk.Contracts.PromptPay.Models
{
    public class BillPayment
    {
        public string NationalIdOrTaxId { get; set; }
        public string Suffix { get; set; }
        public string Reference1 { get; set; }
        public string Reference2 { get; set; }
        public bool DomesticMerchant { get; set; } = true;
    }
}
