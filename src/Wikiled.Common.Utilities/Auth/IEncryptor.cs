namespace Wikiled.Common.Utilities.Auth
{
    public interface IEncryptor
    {
        string Salt { get; }

        string EncryptString(string plainText, string passPhrase);

        string DecryptString(string cipherText, string passPhrase);
    }
}