using PhoneStation.PhoneNumber;
using System;
using System.Globalization;

namespace PhoneStation.StationLogs
{
    class LogAction : ILogAction
    {
        public IPhoneNumber Caller { get; }
        public IPhoneNumber Receiver { get; }
        public DateTime Start { get; }
        public DateTime End { get; }
        public TimeSpan Duration => End - Start;
        public double MoneySpent { get; }

        public LogAction(IPhoneNumber caller, IPhoneNumber receiver, DateTime start, DateTime end, double moneySpent)
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
