using PhoneStation.PhoneNumber;
using System;

namespace PhoneStation.Station
{
    class OngoingCall
    {
        public string Caller { get; }
        public string Receiver { get; }
        public DateTime Start { get; }

        public OngoingCall(string caller, string receiver, DateTime start)
        {
            Caller = caller;
            Receiver = receiver;
            Start = start;
        }
    }
}
