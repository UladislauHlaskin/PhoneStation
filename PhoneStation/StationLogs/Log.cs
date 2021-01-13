using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneStation.StationLogs
{
    public class Log
    {
        ICollection<ILogAction> Actions { get; }
        void Add(ILogAction action)
        {

        }
    }
}
