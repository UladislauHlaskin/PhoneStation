using PhoneStation.PhoneNumber;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneStation.StationLogs
{
    public class Log 
    {
        public ICollection<ILogAction> Actions { get; }

        public Log()
        {
            Actions = new List<ILogAction>();
        }

        void Add(ILogAction action)
        {
            Actions.Add(action);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var action in Actions)
            {
                sb.AppendLine(action.ToString());
            }
            return sb.ToString();
        }
    }
}
