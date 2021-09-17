# SlackBufferedLogger

**SlackBufferedLogger** is a custom logging provider that can be used to send buffered logs to Slack.

This logger takes the approach of sending as few messages as possible, by grouping them by identical texts, and sending these groups in a condensed format with as few lines as possible.

Messages are not sent immediately, but are added to an observable sequence with identical messages. This buffer is released after a period of calm where no new identical messages have been added.

## Example

Given a list of logs emitted during a period of 20 seconds:
```
System.DivideByZeroException: Attempted to divide by zero
System.DivideByZeroException: Attempted to divide by zero
System.DivideByZeroException: Attempted to divide by zero
System.InvalidOperationException: Argument "age" cannot be negative
```

Will result in two messages being sent to Slack:

```
Received 3 items last 20 s: System.DivideByZeroException: Attempted to divide by zero
Received 1 item last 20 s: System.InvalidOperationException: Argument "age" cannot be negative
```

## Installation from NuGet

```
$ dotnet add package SlackBufferedLogger
```

## Setup

In `Program.cs`:

```
public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); }).ConfigureLogging(
                    builder =>
                        builder.AddSlackBufferedLogger(configuration =>
                        {
                            configuration.WebhookUrl =
                                "https://hooks.slack.com/services/xxx/yyy/zzz";
                        }));
```