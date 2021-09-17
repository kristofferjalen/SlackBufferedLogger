using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace SlackBufferedLogger
{
    [ProviderAlias("SlackBufferedLogger")]
    public sealed class SlackBufferedLoggerProvider : ILoggerProvider
    {
        private readonly ConcurrentDictionary<string, SlackBufferedLogger> _loggers = new();
        private readonly IDisposable _onChangeToken;
        private readonly ISlackWebhookService _slackWebhookService;
        private SlackBufferedLoggerConfiguration _currentConfig;

        public SlackBufferedLoggerProvider(
            IOptionsMonitor<SlackBufferedLoggerConfiguration> config, ISlackWebhookService slackWebhookService)
        {
            _ = config ?? throw new ArgumentNullException(nameof(config));

            _slackWebhookService = slackWebhookService ?? throw new ArgumentNullException(nameof(slackWebhookService));
            _currentConfig = config.CurrentValue ?? throw new ArgumentException(nameof(config.CurrentValue));
            _onChangeToken = config.OnChange(updatedConfig => _currentConfig = updatedConfig);
        }

        public ILogger CreateLogger(string categoryName)
        {
            _ = categoryName ?? throw new ArgumentNullException(nameof(categoryName));

            if (string.IsNullOrEmpty(categoryName.Trim()))
            {
                throw new ArgumentException(nameof(categoryName));
            }

            return _loggers.GetOrAdd(categoryName,
                name => new SlackBufferedLogger(_slackWebhookService, name, GetCurrentConfig));
        }

        public void Dispose()
        {
            _loggers.Clear();
            _onChangeToken.Dispose();
        }

        private SlackBufferedLoggerConfiguration GetCurrentConfig() => _currentConfig;
    }
}