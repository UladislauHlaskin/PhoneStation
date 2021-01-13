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
                    return PortState.UnPlagged;
                }
                else if (IsBusy)
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
        public bool IsBusy { get; set; }
        public IPort ConnectedCallPort { get; set; }

        public Port(IStation station)
        {
            Station = station;
            station.AvailablePorts.Add(this);
        }

        public void SendRequestToCall(string callerNumber, string receiverNumber)
        {
            Station.SendRequestToCall(callerNumber, receiverNumber);
        }

        public void SendErrorMessage(string message)
        {
            Terminal.SendErrorMessage(message);
        }

        public void UnplugTerminal()
        {
            Terminal = null;
            IsBusy = false;
            ConnectedCallPort.ConnectedCallPort = null;
            ConnectedCallPort = null;
        }

        public void SendRequestToAnswer(string callerNumber)
        {
            Terminal.SendRequestToAnswer(callerNumber);
        }
    }
}
