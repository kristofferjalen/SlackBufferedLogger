using System;
using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using SlackBufferedLogger.Extensions;
using SlackBufferedLogger.Models;

namespace SlackBufferedLogger.Tests.Extensions
{
    [TestFixture]
    public static class SlackWebhookExtensionsTests
    {
        [Test]
        public static void CanCallBufferUntilCalm()
        {
            var source = Substitute.For<IObservable<string>>();
            var calmDuration = new TimeSpan();
            const int maxCount = 1825414700;
            var result = source.BufferUntilCalm(calmDuration, maxCount);
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public static void CannotCallBufferUntilCalmWithNullSource()
        {
            Assert.Throws<ArgumentNullException>(() =>
                default(IObservable<string>)!.BufferUntilCalm(new TimeSpan(), 1260409863));
        }

        [Test]
        public static void CanCallByText()
        {
            var collection = new List<(string, IEnumerable<SlackWebhookMessage>)>
            {
                ("https://hooks.slack.com/services/xxx/yyy/zzz", new SlackWebhookMessage[] { new("foo"), new("foo"), new("foo") }),
                ("https://hooks.slack.com/services/xxx/yyy/zzz", new SlackWebhookMessage[] { new("foo"), new("foo"), new("bar") }),
                ("https://hooks.slack.com/services/xxx/yyy/zzz", new SlackWebhookMessage[] { new("bar") }),
            };

            var result = collection.ByText().ToList();

            Assert.That(result, Has.Count.EqualTo(4));
            Assert.AreEqual(result[0].count, 3);
            Assert.AreEqual(result[1].count, 2);
            Assert.AreEqual(result[2].count, 1);
            Assert.AreEqual(result[3].count, 1);
        }

        [Test]
        public static void CannotCallByTextWithNullCollection()
        {
            Assert.Throws<ArgumentNullException>(() =>
                default(IEnumerable<(string webhookUrl, IEnumerable<SlackWebhookMessage> messages)>)!.ByText());
        }

        [Test]
        public static void CanCallByWebhookUrl()
        {
            var collection = new[]
            {
                ("https://hooks.slack.com/services/xxx/yyy/zzz", new[] { "foo", "bar" }),
                ("https://hooks.slack.com/services/xxx/yyy/zzz", new[] { "foo", "bar" }),
                ("https://hooks.slack.com/services/aaa/bbb/ccc", new[] { "foo", "bar" })
            };

            var result = collection.ByWebhookUrl();

            Assert.That(result.ToList(), Has.Count.EqualTo(2));
        }

        [Test]
        public static void CannotCallByWebhookUrlWithNullCollection()
        {
            Assert.Throws<ArgumentNullException>(() => default(IEnumerable<(string, string)>)!
                .ByWebhookUrl());
        }
    }
}