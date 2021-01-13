using PhoneStation.PhoneNumber;
using PhoneStation.Port;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneStation.Terminal
{
    public class Terminal : ITerminal
    {
        public string Name { get; }

        public IPort Port { get; private set; }

        public IPhoneNumber PhoneNumber { get; set; }

        public bool IsReceivingCall { get; private set; }

        public Terminal(string name)
        {
            Name = name;
        }

        public void SendRequestToAnswer(string callerNumber)
        {
            Console.WriteLine($"{callerNumber} is calling!");
            IsReceivingCall = true;
        }

        public void Answer()
        {
            if (IsReceivingCall)
            {
                //TODO Log append
            }
        }

        public void Call(string receiverNumber)
        {
            if (receiverNumber == PhoneNumber.Number)
            {
                throw new InvalidOperationException("You cannot call yourself.");
            }
            else if (Port != null)
            {
                Port.IsBusy = true;
                Port.SendRequestToCall(PhoneNumber.Number, receiverNumber);
                // TODO Log append
            }
            else
            {
                throw new InvalidOperationException("Unable to call, the terminal isn't connected to any port.");
            }    
        }

        public void Drop()
        {
            if (IsReceivingCall)
            {
                //TODO Log append
                Port.IsBusy = false;
                IsReceivingCall = false;
            }
        }

        public void Plug(IPort port)
        {
            if (port.PortState == PortState.UnPlagged)
            {
                Port = port;
                Port.Terminal = this;
            }
            else
            {
                throw new AccessViolationException("The port is taken, cannot plug.");
            }
        }

        public void Unplug()
        {
            Port.UnplugTerminal();
            Port = null;
        }

        public void SendErrorMessage(string message)
        {
            Console.WriteLine(message);
            Drop();
        }
    }
}
