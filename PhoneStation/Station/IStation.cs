using PhoneStation.Port;
using PhoneStation.StationLogs;
using PhoneStation.Terminal;
using System.Collections.Generic;

namespace PhoneStation.Station
{
    public interface IStation
    {
        IList<IPort> AvailablePorts { get; }
        Log Log { get; }
        decimal Tariff { get; }
        void SendRequestToCall(string callerNumber, string receiverNumber);
        void StartOnGoingCall(string caller, string receiver);
        void EndOngoingCall(string droppingNumber, int callDurationMinutes);
    }
}
