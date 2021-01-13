using PhoneStation.Port;
using PhoneStation.StationLogs;
using System.Collections.Generic;
using System.Linq;

namespace PhoneStation.Station
{
    public class Station : IStation
    {
        public IList<IPort> AvailablePorts { get; private set; }

        public Log Log { get; }

        public double Tariff { get; private set; }

        private int _defaultPortCapacity = 10;
        private double _defaultTariff = 1;

        public Station()
        {
            AvailablePorts = new List<IPort>();
            for (int i = 0; i < _defaultPortCapacity; i++)
            {
                AvailablePorts.Add(new Port.Port(this));
            }
            Log = new Log();
            Tariff = _defaultTariff;
        }

        public Station(int portCapacity, double tariff)
        {
            AvailablePorts = new List<IPort>();
            for (int i = 0; i < portCapacity; i++)
            {
                AvailablePorts.Add(new Port.Port(this));
            }
            Log = new Log();
            Tariff = tariff;
        }

        public void SendRequestToCall(string callerNumber, string receiverNumber)
        {
            var callerPort = AvailablePorts.Where(p => p.Terminal.PhoneNumber.Number == callerNumber).FirstOrDefault();
            var receiverPort = AvailablePorts.Where(p => p.Terminal.PhoneNumber.Number == receiverNumber).FirstOrDefault();
            if (receiverPort != null)
            {
                if (receiverPort.PortState == PortState.Free)
                {
                    ConnectPorts(callerPort, receiverPort);
                    receiverPort.SendRequestToAnswer(callerNumber);
                }
                else
                {
                    callerPort.SendErrorMessage("The receiver is busy. Try to call later.");
                }
            }
            else
            {
                callerPort.SendErrorMessage("We cannot reach the receiver. The receiver isn't connected to any port or doesn't exist.");
            }
        }

        void ConnectPorts(IPort caller, IPort receiver)
        {
            caller.ConnectedCallPort = receiver;
            receiver.ConnectedCallPort = caller;
            receiver.IsBusy = true;
        }
    }
}
