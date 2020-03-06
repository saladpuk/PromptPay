namespace Saladpuk.PromptPay
{
    public interface ICyclicRedundancyCheck
    {
        string ComputeChecksum(string data);
    }
}
