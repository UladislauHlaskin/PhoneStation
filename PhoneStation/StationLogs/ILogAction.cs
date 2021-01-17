using PhoneStation.PhoneNumber;
using System;

namespace PhoneStation.StationLogs
{
    public interface ILogAction
    {
        IStationUser Caller { get; }
        IStationUser Receiver { get; }
        DateTime Start { get; }
        DateTime End { get; }
        TimeSpan Duration { get; }
        decimal MoneySpent { get; }
    }
}
