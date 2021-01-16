using PhoneStation.PhoneNumber;
using PhoneStation.Port;
using System;

namespace PhoneStation.Terminal
{
    public class Terminal : ITerminal
    {
        public IPort Port { get; set; }
        public IPhoneNumber PhoneNumber { get; set; }

        public event TerminalEventHandler TryingToCall;
        public event TerminalEventHandler ReceivingCall;
        public event TerminalEventHandler Answering;
        public event TerminalEventHandler Dropping;
        public event TerminalEventHandler UnableTocall;

        private string _someonesNumber;

        public Terminal(IPhoneNumber phoneNumber)
        {
            PhoneNumber = phoneNumber;
        }

        public void ReceiveCallNotification(string callerNumber)
        {
            _someonesNumber = callerNumber;
            ReceivingCall?.Invoke(this, new TerminalEventArgs(callerNumber));
        }

        public void Answer()
        {
            Answering?.Invoke(this, new TerminalEventArgs(_someonesNumber));
            Port.Station.StartOnGoingCall(_someonesNumber, PhoneNumber.Number);
        }

        public void Call(string receiverNumber)
        {
            TryingToCall?.Invoke(this, new TerminalEventArgs(receiverNumber));

            if (Port == null)
            {
                throw new NullReferenceException($"{PhoneNumber.UserName} is unable to make a call. The phone isn't connected to any port.");
            }
            if (Port.PortState == PortState.Busy)
            {
                throw new InvalidOperationException($"{PhoneNumber.UserName} cannot call, his port is taken.");
            }
            if (receiverNumber == PhoneNumber.Number)
            {
                throw new InvalidOperationException($"{PhoneNumber.UserName} cannot call himself (herself).");
            }
            else if (Port != null)
            {
                Port.SendRequestToCall(PhoneNumber.Number, receiverNumber);
            }
            else
            {
                throw new InvalidOperationException($"{PhoneNumber.UserName} is unable to call, the terminal isn't connected to any port.");
            }
            _someonesNumber = receiverNumber;
        }

        public void Drop()
        {
            Dropping?.Invoke(this, new TerminalEventArgs(_someonesNumber));
            _someonesNumber = null;
            Port.Station.EndOngoingCall(PhoneNumber.Number, 2);
        }

        public void Drop(int callDurationMinutes)
        {
            Dropping?.Invoke(this, new TerminalEventArgs(_someonesNumber));
            _someonesNumber = null;
            Port.Station.EndOngoingCall(PhoneNumber.Number, callDurationMinutes);
        }

        public void Plug(IPort port)
        {
            port.PlugTerminal(this);
        }

        public void Unplug()
        {
            Port.UnplugTerminal();
            Port = null;
            _someonesNumber = null;
        }

        public void UnableToCallMessage(string message)
        {
            UnableTocall?.Invoke(this, new TerminalEventArgs(message));
        }

        public override string ToString()
        {
            return $"{PhoneNumber}, {Port.PortState.ToString()}";
        }
    }
}
