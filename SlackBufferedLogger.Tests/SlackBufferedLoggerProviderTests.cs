using System;
using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework;

namespace SlackBufferedLogger.Tests
{
    [TestFixture]
    public class SlackBufferedLoggerProviderTests
    {
        [SetUp]
        public void SetUp()
        {
            _config = Substitute.For<IOptionsMonitor<SlackBufferedLoggerConfiguration>>();
            _config.CurrentValue.Returns( new SlackBufferedLoggerConfiguration
            {
                Source = "foo",
                WebhookUrl = "bar"
            });
            _slackWebhookService = Substitute.For<ISlackWebhookService>();
            _testClass = new SlackBufferedLoggerProvider(_config, _slackWebhookService);
        }

        private SlackBufferedLoggerProvider _testClass;
        private IOptionsMonitor<SlackBufferedLoggerConfiguration> _config;
        private ISlackWebhookService _slackWebhookService;

        [Test]
        public void CanConstruct()
        {
            var instance = new SlackBufferedLoggerProvider(_config, _slackWebhookService);
            Assert.That(instance, Is.Not.Null);
        }

        [Test]
        public void CannotConstructWithNullConfig()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new SlackBufferedLoggerProvider(default!, Substitute.For<ISlackWebhookService>()));
        }

        [Test]
        public void CannotConstructWithNullSlackWebhookService()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new SlackBufferedLoggerProvider(Substitute.For<IOptionsMonitor<SlackBufferedLoggerConfiguration>>(),
                    default!));
        }

        [Test]
        public void CanCallCreateLogger()
        {
            var categoryName = "TestValue1085042697";
            var result = _testClass.CreateLogger(categoryName);
            Assert.That(result, Is.Not.Null);
        }

        [TestCase(null)]
        public void CannotCallCreateLoggerWithInvalidCategoryName(string value)
        {
            Assert.Throws<ArgumentNullException>(() => _testClass.CreateLogger(value));
        }

        [TestCase("")]
        [TestCase("   ")]
        public void CannotCallCreateLoggerWithEmptyCategoryName(string value)
        {
            Assert.Throws<ArgumentException>(() => _testClass.CreateLogger(value));
        }
    }
}