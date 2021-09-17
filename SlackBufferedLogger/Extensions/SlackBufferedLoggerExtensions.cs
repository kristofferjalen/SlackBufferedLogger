using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;

namespace SlackBufferedLogger.Extensions
{
    public static class SlackBufferedLoggerExtensions
    {
        public static ILoggingBuilder AddSlackBufferedLogger(
            this ILoggingBuilder builder)
        {
            _ = builder ?? throw new ArgumentNullException(nameof(builder));

            builder.AddConfiguration();

            builder.Services.AddSingleton<ISlackWebhookService, SlackWebhookService>();

            builder.Services.TryAddEnumerable(
                ServiceDescriptor.Singleton<ILoggerProvider, SlackBufferedLoggerProvider>());

            LoggerProviderOptions.RegisterProviderOptions
                <SlackBufferedLoggerConfiguration, SlackBufferedLoggerProvider>(builder.Services);

            return builder;
        }

        public static ILoggingBuilder AddSlackBufferedLogger(this ILoggingBuilder builder, Action<SlackBufferedLoggerConfiguration> configure)
        {
            _ = builder ?? throw new ArgumentNullException(nameof(builder));

            builder.AddSlackBufferedLogger();

            builder.Services.Configure(configure);

            return builder;
        }
    }
}