using NUnit.Framework;
using Wikiled.Common.Utilities.Auth;

namespace Wikiled.Common.Utilities.Tests.Auth
{
    [TestFixture]
    public class SimpleEncryptorTests
    {
        private SimpleEncryptor instance;

        [SetUp]
        public void SetUp()
        {
            instance = CreateInstance();
        }

        [TestCase("Encrypt", "Pass")]
        public void Encrypt(string text, string pass)
        {
            var encrypted = instance.EncryptString(text, pass);
            Assert.AreNotEqual(text, encrypted);
            var decrypted = instance.DecryptString(encrypted, pass);
            Assert.AreEqual(text, decrypted);
        }

        private SimpleEncryptor CreateInstance()
        {
            return new SimpleEncryptor();
        }
    }
}
