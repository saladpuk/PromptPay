namespace Saladpuk.Contracts
{
    public interface IQrReader
    {
        IQrInfo Read(string code);
    }
}
