using PhoneStation.PhoneNumber;
using PhoneStation.Port;
using System;

namespace PhoneStation.Terminal
{
    public interface ITerminal
    {
        IPort Port { get; }
        IPhoneNumber PhoneNumber { get; set; }
        void Answer();
        void Call(string numberToCall);
        void Drop();
        void Plug(IPort port);
        void Unplug();
        void SendErrorMessage(string message);
        void SendRequestToAnswer(string callerNumber);
    }
}
