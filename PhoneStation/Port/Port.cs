using PhoneStation.Station;
using PhoneStation.Terminal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneStation.Port
{
    public class Port : IPort
    {
        public PortState PortState
        {
            get
            {
                if(Terminal == null)
                {
                    return PortState.UnPlugged;
                }
                else if (ConnectedCallPort != null)
                {
                    return PortState.Busy;
                }
                else
                {
                    return PortState.Free;
                }
            }
        }

        public ITerminal Terminal { get; set; }
        public IStation Station { get; }
        public IPort ConnectedCallPort { get; set; }

        public Port(IStation station)
        {
            Station = station;
        }

        public void DropConnection()
        {
            ConnectedCallPort.ConnectedCallPort = null;
            ConnectedCallPort = null;
        }

        public void SendRequestToCall(string callerNumber, string receiverNumber)
        {
            Station.SendRequestToCall(callerNumber, receiverNumber);
        }

        public void SendErrorMessage(string message)
        {
            Terminal.UnableToCallMessage(message);
        }

        public void PlugTerminal(ITerminal terminal)
        {
            if (PortState == PortState.UnPlugged)
            {
                Terminal = terminal;
                Terminal.Port = this;
            }
            else
            {
                throw new InvalidOperationException("The port is taken, cannot plug.");
            }
        }

        public void UnplugTerminal()
        {
            Terminal = null;
            ConnectedCallPort.ConnectedCallPort = null;
            ConnectedCallPort = null;
        }

        public void SendRequestToAnswer(string callerNumber)
        {
            Terminal.ReceiveCallNotification(callerNumber);
        }
    }
}
