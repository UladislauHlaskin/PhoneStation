using System;
using System.Collections.Generic;
using System.Linq;
using PhoneStation.Station;
using PhoneStation.PhoneNumber;
using PhoneStation.Terminal;
using System.Text;
/*
Разработать набор классов для моделирования работы автоматический телефонной станции (АТС) и простейшей биллинговой системы. 

Компания-оператор АТС заключает договора с клиентами, 
присваивает им абонентские номера, 
предоставляет порты для подключения абонентских терминалов и 
выдаёт каждому абоненту терминал (телефон). 
Каждый терминал соответствует только одному номеру. 
Абонент может самостоятельно отключать/подключать телефон к порту станции (станция умеет отслеживать изменения состояния порта – отключен, подключен, звонок, и т.п.).
Абоненты могут звонить друг другу только пределах станции. 
Звонки платные, для всех абонентов применяется один тарифный план. 

Абонент может просмотреть детализированный отчет по звонкам (продолжительность/стоимость/абонент) как минимум за предыдущий месяц, выполнять фильтрацию по дате звонка, сумме, абоненту. 


TODO
* log
* money
*/
namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            int portCapacity = 10;
            double defaultTariff = 0.1;
            IStation station = new Station(portCapacity, defaultTariff);
            var terminals = GetTerminals();
            PlugPorts(station, terminals);

            var johnsPhone = GetTerminalByName(terminals, "John");
            var janesPhone = GetTerminalByName(terminals, "Jane");
            var anonymousPhone = GetTerminalByName(terminals, "Anonymous");
            var michaelsPhone = GetTerminalByName(terminals, "Michael");
            var rachelsPhone = GetTerminalByName(terminals, "Rachel");
            var jacksPhone = new Terminal(new PhoneNumber("001-01-01", "Jack"));

            Call(michaelsPhone, johnsPhone);
            johnsPhone.Answer();
            michaelsPhone.Drop();
            Call(anonymousPhone, michaelsPhone);
            Call(anonymousPhone, rachelsPhone);
            michaelsPhone.Drop();
            Call(janesPhone, rachelsPhone);
            rachelsPhone.Answer();
            janesPhone.Drop();
            Call(jacksPhone, johnsPhone);
            Call(johnsPhone, jacksPhone);
        }

        static IList<ITerminal> GetTerminals()
        {
            List<ITerminal> terminals = new List<ITerminal>();
            terminals.Add(GetTerminal(new PhoneNumber("123-00-00", "John", 10), ConsoleTerminalHandler.OnTryToCallBeep));
            terminals.Add(GetTerminal(new PhoneNumber("123-00-01", "Jane", 15), ConsoleTerminalHandler.OnTryToCallCustomSong));
            terminals.Add(GetTerminal(new PhoneNumber("123-00-03", "Anonymous", 10), ConsoleTerminalHandler.OnTryToCallBeep));
            terminals.Add(GetTerminal(new PhoneNumber("321-00-01", "Michael", 10), ConsoleTerminalHandler.OnTryToCallBeep));
            terminals.Add(GetTerminal(new PhoneNumber("431-01-01", "Rachel", 20), ConsoleTerminalHandler.OnTryToCallCustomSong));
            return terminals;
        }

        static ITerminal GetTerminal(IPhoneNumber phoneNumber, TerminalEventHandler terminalEventHandler)
        {
            var terminal = new Terminal(new PhoneNumber(phoneNumber.Number, phoneNumber.UserName, phoneNumber.Money));
            terminal.TryingToCall += terminalEventHandler;
            terminal.ReceivingCall += ConsoleTerminalHandler.OnReceiveCallNotification;
            terminal.Answering += ConsoleTerminalHandler.OnAnswer;
            terminal.Dropping += ConsoleTerminalHandler.OnDrop;
            return terminal;
        }

        static void PlugPorts(IStation station, IList<ITerminal> terminals)
        {
            for(int i = 0; i < terminals.Count; i++)
            {
                if (i < station.AvailablePorts.Count)
                {
                    try
                    {
                        //station.AvailablePorts[i].PlugTerminal(terminals[i]);
                        // Абонент может самостоятельно отключать/ подключать телефон к порту станции (от имени терминала я полагаю?)
                        terminals[i].Plug(station.AvailablePorts[i]);
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }

        static ITerminal GetTerminalByName(IList<ITerminal> terminals, string name)
        {
            return terminals.FirstOrDefault(t => t.PhoneNumber.UserName == name);
        }

        static void Call(ITerminal sender, ITerminal receiver)
        {
            try
            {
                sender.Call(receiver.PhoneNumber.Number);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                //Console.WriteLine(ex.ToString());
            }
        }

    }
}
