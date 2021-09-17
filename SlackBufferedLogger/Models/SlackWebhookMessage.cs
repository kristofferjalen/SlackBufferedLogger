using System;

namespace SlackBufferedLogger.Models
{
    public class SlackWebhookMessage
    {
        public SlackWebhookMessage(string text)
        {
            _ = text ?? throw new ArgumentNullException(nameof(text));

            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentException("Argument cannot be null or whitespace only.");
            }

            Text = text;
        }

        public string Text { get; init; }
    }
}
