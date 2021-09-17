using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SlackBufferedLogger.Extensions;
using SlackBufferedLogger.Models;

namespace SlackBufferedLogger
{
    internal class SlackWebhookService : ISlackWebhookService
    {
        private const int MaxCount = 100;
        private readonly TimeSpan _calmDuration = TimeSpan.FromSeconds(20);
        private readonly HttpClient _client;
        private readonly Subject<(string, SlackWebhookMessage)> _messages;

        public SlackWebhookService()
        {
            _client = new HttpClient();
            _messages = new Subject<(string, SlackWebhookMessage)>();
            _messages
                .BufferUntilCalm(_calmDuration, MaxCount, _calmDuration)
                .Subscribe(ProcessBuffer);
        }

        public void BufferMessage(string webhookUrl, SlackWebhookMessage message)
        {
            _ = message ?? throw new ArgumentNullException(nameof(message));
            _ = webhookUrl ?? throw new ArgumentNullException(nameof(webhookUrl));

            if (string.IsNullOrEmpty(webhookUrl.Trim()))
            {
                throw new ArgumentException(nameof(webhookUrl));
            }

            _messages.OnNext((webhookUrl, message));
        }

        private void ProcessBuffer(IList<(string webhookUrl, SlackWebhookMessage message)> buffer) => Task.WhenAll(
            buffer.ByWebhookUrl().ByText().Select(x => SendMessageToSlack(x.webhookUrl, x.count, x.message)));

        private Task<HttpResponseMessage> SendMessageToSlack(string webhookUrl, int count, string text)
        {
            var requestUri = new Uri(webhookUrl);

            var message =
                $"Received {count} {(count > 1 ? "items" : "item")} last {_calmDuration.TotalSeconds} s: {text}";

            var payload = new { text = message };

            var json = JsonSerializer.Serialize(payload);

            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

            var task = _client.PostAsync(requestUri, stringContent);

            return task;
        }
    }
}