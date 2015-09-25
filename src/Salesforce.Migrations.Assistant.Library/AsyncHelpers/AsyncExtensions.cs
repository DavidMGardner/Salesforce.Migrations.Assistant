using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Salesforce.Migrations.Assistant.Library.AsyncHelpers
{
    static public class AsyncExtensions
    {
        public class PollingResultHelper : IPollingResult
        {
            public T PollForResultWrapper<T>(int timesToRepeat, int totalTimeLimitInSeconds, int retryIntervalInSeconds, Func<T> action, Func<T, bool> predicate)
            {
                return this.PollForResultWrapper<T>(timesToRepeat, totalTimeLimitInSeconds, retryIntervalInSeconds, action, predicate, CancellationToken.None);
            }

            public T PollForResultWrapper<T>(int timesToRepeat, int totalTimeLimitInSeconds, int retryIntervalInSeconds, Func<T> action, Func<T, bool> predicate, CancellationToken cancellationToken)
            {
                for (int index = 0; index < timesToRepeat; ++index)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    T obj = PollForResult(totalTimeLimitInSeconds, retryIntervalInSeconds, action, predicate);
                    if (obj != null)
                        return obj;
                }
                throw new DownloadFilesResultNullException();
            }

            public T PollForResult<T>(int totalTimeLimitInSeconds, int retryIntervalInSeconds, Func<T> action, Func<T, bool> predicate)
            {
                return this.PollForResult<T>(totalTimeLimitInSeconds, retryIntervalInSeconds, action, predicate, CancellationToken.None);
            }

            public T PollForResult<T>(int totalTimeLimitInSeconds, int retryIntervalInSeconds, Func<T> action, Func<T, bool> predicate, CancellationToken cancellationToken)
            {
                T obj = default(T);
                int num = 0;
                while (num <= totalTimeLimitInSeconds)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    obj = action();
                    if (predicate(obj))
                        return obj;
                    num += retryIntervalInSeconds;
                    Thread.Sleep(1000 * retryIntervalInSeconds);
                }
                return obj;
            }
        }
    }
}
