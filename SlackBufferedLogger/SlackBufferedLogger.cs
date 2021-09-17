using System;
using Microsoft.Extensions.Logging;
using SlackBufferedLogger.Models;

namespace SlackBufferedLogger
{
    public class SlackBufferedLogger : ILogger
    {
        private readonly Func<SlackBufferedLoggerConfiguration> _getCurrentConfig;
        private readonly string _name;
        private readonly ISlackWebhookService _slackWebhookService;

        public SlackBufferedLogger(ISlackWebhookService slackWebhookService, string name, 
            Func<SlackBufferedLoggerConfiguration> getCurrentConfig)
        {
            _slackWebhookService = slackWebhookService ?? throw new ArgumentNullException(nameof(slackWebhookService));
            _name = name ?? throw new ArgumentNullException(nameof(name));
            _getCurrentConfig = getCurrentConfig ?? throw new ArgumentNullException(nameof(getCurrentConfig));

            if (string.IsNullOrEmpty(_name.Trim()))
            {
                throw new ArgumentException(nameof(_name));
            }

            _ = _getCurrentConfig().Source ??
                throw new ArgumentException(nameof(SlackBufferedLoggerConfiguration.Source));

            _ = _getCurrentConfig().WebhookUrl ??
                throw new ArgumentException(nameof(SlackBufferedLoggerConfiguration.WebhookUrl));
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
            Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel)) return;

            SlackBufferedLoggerConfiguration config = _getCurrentConfig();

            var text = $"{config.Source} - {formatter(state, exception)}";

            var slackWebhookMessage = new SlackWebhookMessage(text);

            _slackWebhookService.BufferMessage(config.WebhookUrl, slackWebhookMessage);
        }

        public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None;

        public IDisposable BeginScope<TState>(TState state) => default!;
    }
}