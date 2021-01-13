using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhoneStation.Station;
using PhoneStation.PhoneNumber;
using PhoneStation.Terminal;
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
* events
* log
* exception handling
*/
namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            int portCapacity = 10;
            double defaultTariff = 0.1;
            IStation station = new Station(portCapacity, defaultTariff);
            var phoneNumbers = GetPhoneNumbers();
            var terminals = GetTerminals();
            ConnectTerminalsAndPhoneNumbers(terminals, phoneNumbers);
            PlugPorts(station, terminals);
        }

        static IList<IPhoneNumber> GetPhoneNumbers()
        {
            List<IPhoneNumber> phoneNumbers = new List<IPhoneNumber>();
            phoneNumbers.Add(new PhoneNumber("123-00-00", "John", 10));
            phoneNumbers.Add(new PhoneNumber("123-00-01", "Jane", 15));
            phoneNumbers.Add(new PhoneNumber("123-00-03", "Anonymous", 10));
            phoneNumbers.Add(new PhoneNumber("321-00-01", "Michael", 10));
            phoneNumbers.Add(new PhoneNumber("431-01-01", "Rachel", 20));
            return phoneNumbers;
        }

        static IList<ITerminal> GetTerminals()
        {
            List<ITerminal> terminals = new List<ITerminal>();
            terminals.Add(new Terminal("John's Nokia"));
            terminals.Add(new Terminal("Jane's Motorola"));
            terminals.Add(new Terminal("Anonymous' IPhone"));
            terminals.Add(new Terminal("Michael's Samsung"));
            terminals.Add(new Terminal("Rachel's Sony"));
            return terminals;
        }

        static void ConnectTerminalsAndPhoneNumbers(IList<ITerminal> terminals, IList<IPhoneNumber> phoneNumbers)
        {
            for(int i = 0; i < terminals.Count; i++)
            {
                if (i < phoneNumbers.Count)
                {
                    terminals[i].PhoneNumber = phoneNumbers[i];
                }
            }
        }

        static void PlugPorts(IStation station, IList<ITerminal> terminals)
        {
            for(int i = 0; i < terminals.Count; i++)
            {
                if (i < station.AvailablePorts.Count)
                {
                    terminals[i].Plug(station.AvailablePorts[i]);
                }
            }
        }

    }
}
