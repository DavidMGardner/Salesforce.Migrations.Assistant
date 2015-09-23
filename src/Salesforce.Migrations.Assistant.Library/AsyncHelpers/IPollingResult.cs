using System;
using System.Threading;

namespace Salesforce.Migrations.Assistant.Library.AsyncHelpers
{
    public interface IPollingResult
    {
        T PollForResultWrapper<T>(int timesToRepeat, int totalTimeLimitInSeconds, int retryIntervalInSeconds, Func<T> action, Func<T, bool> predicate, CancellationToken cancellationToken);
        T PollForResultWrapper<T>(int timesToRepeat, int totalTimeLimitInSeconds, int retryIntervalInSeconds, Func<T> action, Func<T, bool> predicate);
        T PollForResult<T>(int totalTimeLimitInSeconds, int retryIntervalInSeconds, Func<T> action, Func<T, bool> predicate, CancellationToken cancellationToken);
        T PollForResult<T>(int totalTimeLimitInSeconds, int retryIntervalInSeconds, Func<T> action, Func<T, bool> predicate);
    }
}