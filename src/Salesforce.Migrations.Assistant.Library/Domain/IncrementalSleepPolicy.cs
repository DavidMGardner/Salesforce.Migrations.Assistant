using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Salesforce.Migrations.Assistant.Library.Domain
{
    public class FixedSleepPolicy : IIncrementalSleepPolicy
    {
        public int SleepPeriod = 60000;
        public TimeSpan WaitTime()
        {
            return new TimeSpan(SleepPeriod);
        }
    }


    public class IncrementalSleepPolicy : IIncrementalSleepPolicy
    {
        public double FirstTimeoutLimit = 60.0;
        public double SecondTimeoutLimit = 240.0;
        public double ThirdTimeoutLimit = 600.0;
        public double FourthTimeoutLimit = 3600.0;
        public int FirstSleepPeriod = 1000;
        public int SecondSleepPeriod = 5000;
        public int ThirdSleepPeriod = 10000;
        public int FourthSleepPeriod = 60000;
        private readonly Stopwatch _stopwatch;

        public IncrementalSleepPolicy()
        {
            _stopwatch = new Stopwatch();
            _stopwatch.Start();
        }

        public TimeSpan WaitTime()
        {
            var elapsedInSeconds = (_stopwatch.ElapsedMilliseconds);

            if (elapsedInSeconds < FirstTimeoutLimit)
            {
                return new TimeSpan(FirstSleepPeriod);
            }
            if (elapsedInSeconds > FirstTimeoutLimit & elapsedInSeconds < SecondTimeoutLimit)
            {
                return new TimeSpan(SecondSleepPeriod);
            }
            if (elapsedInSeconds > SecondTimeoutLimit & elapsedInSeconds < ThirdTimeoutLimit)
            {
                return new TimeSpan(ThirdSleepPeriod);
            }
            if ((elapsedInSeconds > ThirdTimeoutLimit & elapsedInSeconds < FourthTimeoutLimit))
            {
                return new TimeSpan(FourthSleepPeriod);
            }

            return new TimeSpan(SecondSleepPeriod);
        }
    }

    public interface IIncrementalSleepPolicy
    {
        TimeSpan WaitTime();
    }
}
