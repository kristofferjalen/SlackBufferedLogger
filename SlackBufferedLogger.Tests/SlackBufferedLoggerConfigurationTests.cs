using NUnit.Framework;

namespace SlackBufferedLogger.Tests
{
    [TestFixture]
    public class SlackBufferedLoggerConfigurationTests
    {
        [SetUp]
        public void SetUp()
        {
            _testClass = new SlackBufferedLoggerConfiguration();
        }

        private SlackBufferedLoggerConfiguration _testClass;

        [Test]
        public void CanConstruct()
        {
            var instance = new SlackBufferedLoggerConfiguration();
            Assert.That(instance, Is.Not.Null);
        }

        [Test]
        public void CanSetAndGetSource()
        {
            var testValue = "TestValue1411281868";
            _testClass.Source = testValue;
            Assert.That(_testClass.Source, Is.EqualTo(testValue));
        }

        [Test]
        public void CanSetAndGetWebhookUrl()
        {
            var testValue = "TestValue1136953935";
            _testClass.WebhookUrl = testValue;
            Assert.That(_testClass.WebhookUrl, Is.EqualTo(testValue));
        }
    }
}