using PhoneStation.PhoneNumber;
using PhoneStation.Port;
using System;

namespace PhoneStation.Terminal
{
    public delegate void TerminalEventHandler(object sender, TerminalEventArgs e);
    public interface ITerminal
    {
        IPort Port { get; set; }
        IStationUser PhoneNumber { get; }
        event TerminalEventHandler TryingToCall;
        event TerminalEventHandler ReceivingCall;
        event TerminalEventHandler Answering;
        event TerminalEventHandler Dropping;
        event TerminalEventHandler UnableTocall;

        void Call(string numberToCall);
        void ReceiveCallNotification(string callerNumber);
        void Answer();
        void Drop();
        void Drop(int callDurationMinutes);
        void Plug(IPort port);
        void Unplug();
        void UnableToCallMessage(string message);
    }
}
