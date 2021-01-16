using PhoneStation.PhoneNumber;
using System;

namespace PhoneStation.StationLogs
{
    public interface ILogAction
    {
        IPhoneNumber Caller { get; }
        IPhoneNumber Receiver { get; }
        DateTime Start { get; }
        DateTime End { get; }
        TimeSpan Duration { get; }
        double MoneySpent { get; }
    }
}
