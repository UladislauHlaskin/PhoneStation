using PhoneStation.PhoneNumber;
using System;
using System.Globalization;

namespace PhoneStation.StationLogs
{
    class LogAction : ILogAction
    {
        public IStationUser Caller { get; }
        public IStationUser Receiver { get; }
        public DateTime Start { get; }
        public DateTime End { get; }
        public TimeSpan Duration => End - Start;
        public decimal MoneySpent { get; }

        public LogAction(IStationUser caller, IStationUser receiver, DateTime start, DateTime end, decimal moneySpent)
        {
            Caller = caller;
            Receiver = receiver;
            Start = start;
            End = end;
            MoneySpent = moneySpent;
        }

        public override string ToString()
        {
            return $"{Caller.Number} -> {Receiver.Number}; {Start} - {End} ({Duration}), ${MoneySpent.ToString(CultureInfo.InvariantCulture)}";
        }
    }
}
