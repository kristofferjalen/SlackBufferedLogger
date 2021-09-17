using System;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;

namespace SlackBufferedLogger.Tests
{
    [TestFixture]
    public class SlackBufferedLoggerTests
    {
        [SetUp]
        public void SetUp()
        {
            _slackWebhookService = Substitute.For<ISlackWebhookService>();
            _name = "TestValue900323880";
            _getCurrentConfig = () => new SlackBufferedLoggerConfiguration
            {
                Source = "foo",
                WebhookUrl = "bar"
            };
            _testClass = Substitute.For<SlackBufferedLogger>(_slackWebhookService, _name, _getCurrentConfig);
        }

        private SlackBufferedLogger _testClass;
        private ISlackWebhookService _slackWebhookService;
        private string _name;
        private Func<SlackBufferedLoggerConfiguration> _getCurrentConfig;

        [Test]
        public void CanConstruct()
        {
            var instance = new SlackBufferedLogger(_slackWebhookService, _name, _getCurrentConfig);
            Assert.That(instance, Is.Not.Null);
        }

        [Test]
        public void CannotConstructWithNullSlackWebhookService()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new SlackBufferedLogger(default!, "TestValue1385631984", default!));
        }

        [Test]
        public void CannotConstructWithNullGetCurrentConfig()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new SlackBufferedLogger(Substitute.For<ISlackWebhookService>(), "TestValue655898863", default!));
        }

        [TestCase(null)]
        public void CannotConstructWithNullName(string value)
        {
            Assert.Throws<ArgumentNullException>(() =>
                new SlackBufferedLogger(Substitute.For<ISlackWebhookService>(), value, default!));
        }

        [TestCase("")]
        [TestCase("   ")]
        public void CannotConstructWithEmptyName(string value)
        {
            Assert.Throws<ArgumentException>(() =>
                new SlackBufferedLogger(Substitute.For<ISlackWebhookService>(), value, () => new SlackBufferedLoggerConfiguration()));
        }

        [Test]
        public void CanCallLog()
        {
            const LogLevel logLevel = LogLevel.Trace;
            var eventId = new EventId();
            const string state = "TestValue1482306771";
            var exception = new Exception();
            Func<string, Exception, string> formatter = (_, __) => default!;
            _testClass.Log(logLevel, eventId, state, exception, formatter);
            _testClass.Received().Log(logLevel, eventId, state, exception, formatter);
        }

        [Ignore("Exception should be nullable but is not in ILogger interface?")]
        [Test]
        public void CannotCallLogWithNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                _testClass.Log(LogLevel.Critical, new EventId(), "TestValue137611265", default!, default!));
        }

        [Test]
        [Ignore("Formatter should be nullable but is not in ILogger interface?")]
        public void CannotCallLogWithNullFormatter()
        {
            Assert.Throws<ArgumentNullException>(() =>
                _testClass.Log(LogLevel.None, new EventId(), "TestValue1358173817", new Exception(), default!));
        }

        [Test]
        public void CanCallIsEnabled()
        {
            const LogLevel logLevel = LogLevel.Information;
            var result = _testClass.IsEnabled(logLevel);
            Assert.True(result);
        }

        [Test]
        public void CanCallBeginScope()
        {
            var state = "TestValue1133485723";
            _testClass.BeginScope(state);
            _testClass.Received().BeginScope(state);
        }
    }
}