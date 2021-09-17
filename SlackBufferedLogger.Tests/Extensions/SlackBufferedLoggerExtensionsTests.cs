using System;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using SlackBufferedLogger.Extensions;

namespace SlackBufferedLogger.Tests.Extensions
{
    [TestFixture]
    public static class SlackBufferedLoggerExtensionsTests
    {
        [Test]
        public static void CanCallAddSlackBufferedLoggerWithBuilder()
        {
            var builder = Substitute.For<ILoggingBuilder>();
            var result = builder.AddSlackBufferedLogger();
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public static void CannotCallAddSlackBufferedLoggerWithBuilderWithNullBuilder()
        {
            Assert.Throws<ArgumentNullException>(() => default(ILoggingBuilder)!.AddSlackBufferedLogger());
        }

        [Test]
        public static void CanCallAddSlackBufferedLoggerWithBuilderAndConfigure()
        {
            var builder = Substitute.For<ILoggingBuilder>();
            var result = builder.AddSlackBufferedLogger(_ => { });
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public static void CannotCallAddSlackBufferedLoggerWithBuilderAndConfigureWithNullBuilder()
        {
            Assert.Throws<ArgumentNullException>(() => default(ILoggingBuilder)!.AddSlackBufferedLogger(default!));
        }

        [Test]
        public static void CannotCallAddSlackBufferedLoggerWithBuilderAndConfigureWithNullConfigure()
        {
            Assert.Throws<ArgumentNullException>(
                () => Substitute.For<ILoggingBuilder>().AddSlackBufferedLogger(default!));
        }
    }
}