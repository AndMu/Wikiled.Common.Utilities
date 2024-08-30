using NUnit.Framework;
using NUnit.Framework.Legacy;
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
            ClassicAssert.AreNotEqual(text, encrypted);
            var decrypted = instance.DecryptString(encrypted, pass);
            ClassicAssert.AreEqual(text, decrypted);
        }

        private SimpleEncryptor CreateInstance()
        {
            return new SimpleEncryptor();
        }
    }
}
