using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    public class EventTest
    {
        bool _flag;
        public bool Flag 
        {
            get 
            {
                return _flag;
            }
            set
            {
                _flag = value;
                OnFlagChanged(this, new EventArgs());
            }
        }
        //public event EventHandler FlagChangedEvent;

        private void OnFlagChanged(object sender, EventArgs e)
        {
            //if (FlagChangedEvent != null)
            //    FlagChangedEvent(this, e);
            Console.WriteLine("The flag has been changed.");
        }
    }
}
