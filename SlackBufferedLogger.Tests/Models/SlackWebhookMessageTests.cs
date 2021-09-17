using System;
using NUnit.Framework;
using SlackBufferedLogger.Models;

namespace SlackBufferedLogger.Tests.Models
{
    [TestFixture]
    public class SlackWebhookMessageTests
    {
        [SetUp]
        public void SetUp()
        {
            _text = "TestValue511991935";
            _testClass = new SlackWebhookMessage(_text);
        }

        private SlackWebhookMessage _testClass;
        private string _text;

        [Test]
        public void CanConstruct()
        {
            var instance = new SlackWebhookMessage(_text);
            Assert.That(instance, Is.Not.Null);
        }

        [TestCase(null)]
        public void CannotConstructWithInvalidText(string value)
        {
            Assert.Throws<ArgumentNullException>(() => new SlackWebhookMessage(value));
        }

        [TestCase("")]
        [TestCase("   ")]
        public void CannotConstructWithEmptyText(string value)
        {
            Assert.Throws<ArgumentException>(() => new SlackWebhookMessage(value));
        }
        
        [Test]
        public void TextIsInitializedCorrectly()
        {
            Assert.That(_testClass.Text, Is.EqualTo(_text));
        }
    }
}