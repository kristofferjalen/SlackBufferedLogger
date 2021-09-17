using SlackBufferedLogger.Models;

namespace SlackBufferedLogger
{
    public interface ISlackWebhookService
    {
        void BufferMessage(string webhookUrl, SlackWebhookMessage message);
    }
}