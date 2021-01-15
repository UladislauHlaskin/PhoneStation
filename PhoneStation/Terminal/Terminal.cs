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

        public Terminal(IPhoneNumber phoneNumber)
        {
            PhoneNumber = phoneNumber;
        }

        public void ReceiveCallNotification(string callerNumber)
        {
            ReceivingCall?.Invoke(this, new TerminalEventArgs(callerNumber));
        }

        public void Answer()
        {
            Answering?.Invoke(this, null);
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
        }

        public void Drop()
        {
            Dropping?.Invoke(this, null);
        }

        public void Plug(IPort port)
        {
            port.PlugTerminal(this);
        }

        public void Unplug()
        {
            Port.UnplugTerminal();
            Port = null;
        }

        public void UnableToCallMessage(string message)
        {
            Console.WriteLine(message);
        }

        public override string ToString()
        {
            return $"{PhoneNumber}, {Port.PortState.ToString()}";
        }
    }
}
