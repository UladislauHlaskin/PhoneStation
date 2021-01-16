using PhoneStation.Port;
using PhoneStation.StationLogs;
using PhoneStation.PhoneNumber;
using System;
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
        private IList<OngoingCall> _ongoingCalls;

        public Station()
        {
            AvailablePorts = new List<IPort>();
            for (int i = 0; i < _defaultPortCapacity; i++)
            {
                AvailablePorts.Add(new Port.Port(this));
            }
            Log = new Log();
            _ongoingCalls = new List<OngoingCall>();
            Tariff = _defaultTariff;
        }

        public Station(int portCapacity, double tariff)
        {
            AvailablePorts = new List<IPort>();
            for (int i = 0; i < portCapacity; i++)
            {
                AvailablePorts.Add(new Port.Port(this));
            }
            _ongoingCalls = new List<OngoingCall>();
            Log = new Log();
            Tariff = tariff;
        }

        public void SendRequestToCall(string callerNumber, string receiverNumber)
        {
            var callerPort = AvailablePorts
                .Where(p => p.Terminal.PhoneNumber.Number == callerNumber)
                .FirstOrDefault();
            var receiverPort = AvailablePorts
                .Where(p => p.PortState != PortState.UnPlugged) //без этой строки выдает NullReferenceEx, не знаю, как избавиться это этой проблемы как-то получше
                .Where(p => p.Terminal.PhoneNumber.Number == receiverNumber)
                .FirstOrDefault();
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
        }

        public void StartOnGoingCall(string caller, string receiver)
        {
            _ongoingCalls.Add(new OngoingCall(caller, receiver, DateTime.Now));
        }

        public void EndOngoingCall(string droppingNumber, int callDurationMinutes)
        {
            var call = _ongoingCalls.FirstOrDefault(c => c.Caller == droppingNumber || c.Receiver == droppingNumber);
            if (call != null)
            {
                var caller = AvailablePorts.Select(p => p.Terminal).Select(t => t.PhoneNumber).FirstOrDefault(n => n.Number == call.Caller);
                var receiver = AvailablePorts.Select(p => p.Terminal).Select(t => t.PhoneNumber).FirstOrDefault(n => n.Number == call.Receiver);
                var callEnd = call.Start + new TimeSpan(0, callDurationMinutes, 0);
                var moneySpent = callDurationMinutes * Tariff;
                Log.Actions.Add(new LogAction(caller, receiver, call.Start, callEnd, moneySpent));
                SpendMoney(caller, moneySpent);
                _ongoingCalls.Remove(call);
            }
        }

        void SpendMoney(IPhoneNumber caller, double moneySpent)
        {
            caller.ChangeBalance(-moneySpent);
        }
    }
}
