using System;
using NSubstitute;
using NUnit.Framework;
using SlackBufferedLogger.Models;

namespace SlackBufferedLogger.Tests
{
    [TestFixture]
    public class SlackWebhookServiceTests
    {
        [SetUp]
        public void SetUp()
        {
            _testClass = Substitute.For<SlackWebhookService>();
        }

        private SlackWebhookService _testClass;

        [Test]
        public void CanConstruct()
        {
            var instance = new SlackWebhookService();
            Assert.That(instance, Is.Not.Null);
        }

        [Test]
        public void CanCallBufferMessage()
        {
            const string webhookUrl = "TestValue901885284";
            var message = new SlackWebhookMessage("TestValue1811866897");
            _testClass.BufferMessage(webhookUrl, message);
            _testClass.Received().BufferMessage(webhookUrl, message);
        }

        [Test]
        public void CannotCallBufferMessageWithNullMessage()
        {
            Assert.Throws<ArgumentNullException>(() =>
                _testClass.BufferMessage("TestValue1315602099", default!));
        }

        [TestCase(null)]
        public void CannotCallBufferMessageWithNullWebhookUrl(string value)
        {
            Assert.Throws<ArgumentNullException>(() =>
                _testClass.BufferMessage(value, new SlackWebhookMessage("TestValue1039617829")));
        }

        [TestCase("")]
        [TestCase("   ")]
        public void CannotCallBufferMessageWithInvalidWebhookUrl(string value)
        {
            Assert.Throws<ArgumentException>(() =>
                _testClass.BufferMessage(value, new SlackWebhookMessage("TestValue1039617829")));
        }
    }
}