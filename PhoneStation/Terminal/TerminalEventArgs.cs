using System;

namespace PhoneStation.Terminal
{
    public class TerminalEventArgs : EventArgs
    {
        public string SomeonesNumber { get; }

        public TerminalEventArgs() { }

        public TerminalEventArgs(string someonesNumber)
        {
            SomeonesNumber = someonesNumber;
        }
    }
}
