using System;
using System.Collections.Generic;
using System.Linq;
using PhoneStation.Station;
using PhoneStation.PhoneNumber;
using PhoneStation.Terminal;
using System.Text;
using System.Threading;
using System.Globalization;
/*
Разработать набор классов для моделирования работы автоматический телефонной станции (АТС) и простейшей биллинговой системы. 

Компания-оператор АТС заключает договора с клиентами, присваивает им абонентские номера, предоставляет порты для подключения абонентских терминалов и 
выдаёт каждому абоненту терминал (телефон). Каждый терминал соответствует только одному номеру. 
Абонент может самостоятельно отключать/подключать телефон к порту станции (станция умеет отслеживать изменения состояния порта – отключен, подключен, звонок, и т.п.).
Абоненты могут звонить друг другу только пределах станции. 
Звонки платные, для всех абонентов применяется один тарифный план. 

Абонент может просмотреть детализированный отчет по звонкам (продолжительность/стоимость/абонент) как минимум за предыдущий месяц, выполнять фильтрацию по дате звонка, сумме, абоненту. 

*/
namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            int portCapacity = 10;
            double defaultTariff = 0.01;
            IStation station = new Station(portCapacity, defaultTariff);
            var terminals = GetTerminals();
            PlugPorts(station, terminals);

            var johnsPhone = GetTerminalByName(terminals, "John");
            var janesPhone = GetTerminalByName(terminals, "Jane");
            var anonymousPhone = GetTerminalByName(terminals, "Anonymous");
            var michaelsPhone = GetTerminalByName(terminals, "Michael");
            var rachelsPhone = GetTerminalByName(terminals, "Rachel");
            var jacksPhone = GetTerminal(new PhoneNumber("000-00-01", "Jack", 0), ConsoleTerminalHandler.OnTryToCallBeep);

            //// тест #1 - тесты где абенент может быть недоступен или бросил трубку
            //Call(michaelsPhone, johnsPhone);
            //johnsPhone.Answer();
            //michaelsPhone.Drop();
            //Call(anonymousPhone, michaelsPhone);
            //Call(anonymousPhone, rachelsPhone);
            //michaelsPhone.Drop();
            //Call(janesPhone, rachelsPhone);
            //rachelsPhone.Answer();
            //janesPhone.Drop();
            //Call(jacksPhone, johnsPhone);
            //Call(johnsPhone, jacksPhone);

            // тест #2 - много звонков + лог
            Random randomCaller = new Random(DateTime.Now.Millisecond);
            Thread.Sleep(29);
            Random randomReceiver = new Random(DateTime.Now.Millisecond);
            Thread.Sleep(13);
            Random callDuration = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < 1000; i++)
            {
                var callerIndex = randomCaller.Next(0, terminals.Count);
                var receiverIndex = randomReceiver.Next(0, terminals.Count);
                if (callerIndex != receiverIndex)
                {
                    PerformCall(terminals[callerIndex], terminals[receiverIndex], callDuration.Next(20));
                }
            }
            ShowLog(station, rachelsPhone);
            Console.WriteLine("================balance===================");
            foreach(var t in terminals)
            {
                Console.WriteLine(t.PhoneNumber);
            }
        }

        static IList<ITerminal> GetTerminals()
        {
            List<ITerminal> terminals = new List<ITerminal>();
            terminals.Add(GetTerminal(new PhoneNumber("123-00-00", "John", 22), ConsoleTerminalHandler.OnTryToCallBeep));
            terminals.Add(GetTerminal(new PhoneNumber("123-00-01", "Jane", 18), ConsoleTerminalHandler.OnTryToCallCustomSong));
            terminals.Add(GetTerminal(new PhoneNumber("123-00-03", "Anonymous", 19), ConsoleTerminalHandler.OnTryToCallBeep));
            terminals.Add(GetTerminal(new PhoneNumber("321-00-01", "Michael", 20), ConsoleTerminalHandler.OnTryToCallBeep));
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
            terminal.UnableTocall += ConsoleTerminalHandler.OnUnableToCall;
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

        static void Call(ITerminal caller, ITerminal receiver)
        {
            try
            {
                caller.Call(receiver.PhoneNumber.Number);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void PerformCall(ITerminal caller, ITerminal receiver, int duration)
        {
            try
            {
                caller.Call(receiver.PhoneNumber.Number);
                receiver.Answer();
                caller.Drop(duration);
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void ShowLog(IStation station, ITerminal terminal)
        {
            DateTime from = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime till = from.AddMonths(1).AddDays(-1);
            var logs = station.Log.Actions
                .Where(a => a.Caller == terminal.PhoneNumber || a.Receiver == terminal.PhoneNumber)
                .Where(a => a.Start >= from && a.Start <= till)
                .OrderBy(a => a.Start)
                .ThenBy(a => a.Duration)
                .ToList();
            Console.WriteLine($"======================{terminal.PhoneNumber.UserName}'s logs=======================");
            foreach (var l in logs)
            {
                Console.WriteLine(l);
            }
            var totalSpent = logs.Sum(l => l.MoneySpent);
            var totalDuration = logs.Sum(l => l.Duration.Minutes);
            Console.WriteLine($"TOTAL: call duration: {totalDuration} min, money spent: ${totalSpent.ToString(CultureInfo.InvariantCulture)}");
        }
    }
}
