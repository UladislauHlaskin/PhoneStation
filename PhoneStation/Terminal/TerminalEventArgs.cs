namespace PhoneStation.Terminal
{
    public class TerminalEventArgs
    {
        public string SomeonesNumber { get; set; }

        public TerminalEventArgs(string someonesNumber)
        {
            SomeonesNumber = someonesNumber;
        }
    }
}
