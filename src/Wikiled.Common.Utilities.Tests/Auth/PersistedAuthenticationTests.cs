using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using NUnit.Framework;
using System;
using System.IO;
using System.Net;
using Wikiled.Common.Utilities.Auth;

namespace Wikiled.Common.Utilities.Tests.Auth
{
    [TestFixture]
    public class PersistedAuthenticationTests
    {
        private ILogger<PersistedAuthentication<TestToken>> mockLogger;

        private Mock<IAuthentication<TestToken>> mockAuthentication;

        private PersistedAuthentication<TestToken> instance;

        [SetUp]
        public void SetUp()
        {
            mockLogger = new NullLogger<PersistedAuthentication<TestToken>>();
            mockAuthentication = new Mock<IAuthentication<TestToken>>();
            instance = CreateInstance();
            instance.AuthFile = Path.Combine(TestContext.CurrentContext.TestDirectory, instance.AuthFile);
            File.Delete(instance.AuthFile);
        }

        [Test]
        public void Construct()
        {
            Assert.Throws<ArgumentNullException>(() => new PersistedAuthentication<TestToken>(null, mockAuthentication.Object));
            Assert.Throws<ArgumentNullException>(() => new PersistedAuthentication<TestToken>(mockLogger, null));
        }

        [Test]
        public void Authenticate()
        {
            mockAuthentication.Setup(item => item.Authenticate()).Returns(new TestToken { Token = "Token" });
            var result = instance.Authenticate();
            Assert.AreEqual("Token", result.Token);
        }

        [Test]
        public void AuthenticateTwice()
        {
            mockAuthentication.Setup(item => item.Authenticate()).Returns(new TestToken { Token = "Token" });
            mockAuthentication.Setup(item => item.Refresh(It.IsAny<TestToken>())).Returns(new TestToken { Token = "Token2" });
            var result = instance.Authenticate();
            Assert.AreEqual("Token", result.Token);

            result = instance.Authenticate();
            Assert.AreEqual("Token2", result.Token);
        }

        [Test]
        public void Refresh()
        {
            Assert.Throws<ArgumentNullException>(() => instance.Refresh(null));
            mockAuthentication.Setup(item => item.Refresh(It.IsAny<TestToken>())).Returns(new TestToken { Token = "Token2" });
            var result = instance.Refresh(new TestToken());
            Assert.AreEqual("Token2", result.Token);
        }

        private PersistedAuthentication<TestToken> CreateInstance()
        {
            return new PersistedAuthentication<TestToken>(mockLogger, mockAuthentication.Object);
        }
    }
}
