using PhoneStation.Station;
using PhoneStation.Terminal;
using System;

namespace PhoneStation.Port
{
    public interface IPort
    {
        PortState PortState { get; }
        ITerminal Terminal { get; set; }
        IStation Station { get; }
        bool IsBusy { get; set; }
        IPort ConnectedCallPort { get; set; }

        void SendRequestToCall(string callerNumber, string receiverNumber);
        void SendErrorMessage(string message);
        void UnplugTerminal();
        void SendRequestToAnswer(string callerNumber);
    }
}
