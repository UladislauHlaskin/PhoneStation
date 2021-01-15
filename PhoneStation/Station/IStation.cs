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
        double Tariff { get; }
        void SendRequestToCall(string callerNumber, string receiverNumber);
        //void ConnectPorts(IPort caller, IPort receiver);  // с этим какая-то проблема, не могу разобраться почему
    }
}
