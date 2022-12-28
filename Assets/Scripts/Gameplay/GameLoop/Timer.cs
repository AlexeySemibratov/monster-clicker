using System;
using UniRx;

public static class Timer
{
    public static IObservable<long> From(int fromSeconds, int periodSeconds)
    {
        return Observable.Interval(TimeSpan.FromSeconds(periodSeconds))
            .Select(time => fromSeconds - time)
            .TakeWhile(time => time >= 0);
    }
}
