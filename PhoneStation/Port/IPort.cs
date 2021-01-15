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
        IPort ConnectedCallPort { get; set; }

        void SendRequestToCall(string callerNumber, string receiverNumber);
        void SendErrorMessage(string message);
        void SendRequestToAnswer(string callerNumber);
        void PlugTerminal(ITerminal terminal);
        void UnplugTerminal();
        void DropConnection();
    }
}
