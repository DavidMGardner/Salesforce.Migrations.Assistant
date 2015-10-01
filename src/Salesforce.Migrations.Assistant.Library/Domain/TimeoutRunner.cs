using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Salesforce.Migrations.Assistant.Library.Exceptions;

namespace Salesforce.Migrations.Assistant.Library.Domain
{
    public static class TimeoutRunner
    { 
        public static T RunUntil<T>(Func<T> predicate, Func<T, bool> isCompleted, IIncrementalSleepPolicy timeout, string taskName)
        {
            var result = default(T);
            Action runTask = () => result = predicate();

            try
            {
                var task = Task.Factory.StartNew(runTask);
                if (task.Wait(timeout.WaitTime()))
                {
                    return result;
                }

                throw new TaskTimeoutException(string.Format("'{0}' timed out after {1}", taskName, timeout));
            }
            catch (AggregateException aggregateException)
            {
                throw aggregateException.InnerException;
            }
        }

        public static async Task<TResult> TimeoutAfter<TResult>(this Task<TResult> task, TimeSpan timeout)
        {

            var timeoutCancellationTokenSource = new CancellationTokenSource();

            var completedTask = await Task.WhenAny(task, Task.Delay(timeout, timeoutCancellationTokenSource.Token));
            if (completedTask == task)
            {
                timeoutCancellationTokenSource.Cancel();
                return await task;
            }
            else
            {
                throw new TimeoutException("The operation has timed out.");
            }
        }
    }
}
