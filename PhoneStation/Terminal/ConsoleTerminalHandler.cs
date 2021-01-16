using System;

namespace PhoneStation.Terminal
{
    public class ConsoleTerminalHandler
    {
        public static void OnTryToCallBeep(object sender, TerminalEventArgs e)
        {
            var terminal = sender as ITerminal;
            if (terminal == null)
            {
                return;
            }
            Console.WriteLine($"{terminal.PhoneNumber.UserName} ({terminal.PhoneNumber.Number}) is calling {e.SomeonesNumber}. Beep... Beep... Beep...");
        }

        public static void OnTryToCallCustomSong(object sender, TerminalEventArgs e)
        {
            var terminal = sender as ITerminal;
            if (terminal == null)
            {
                return;
            }
            Console.WriteLine($"{terminal.PhoneNumber.UserName} ({terminal.PhoneNumber.Number}) is calling {e.SomeonesNumber}. A custom song is playing ♪♪♪");
        }

        public static void OnReceiveCallNotification(object sender, TerminalEventArgs e)
        {
            var terminal = sender as ITerminal;
            if (terminal == null)
            {
                return;
            }
            Console.WriteLine($"♫♫♫ {terminal.PhoneNumber.UserName}'s phone: {e.SomeonesNumber} is calling! ♫♫♫");
        }

        public static void OnAnswer(object sender, TerminalEventArgs e)
        {
            var terminal = sender as ITerminal;
            if (terminal == null)
            {
                return;
            }
            if (terminal.Port.PortState == Port.PortState.Busy)
            {
                Console.WriteLine($"{terminal.PhoneNumber.UserName} has answered the call. {terminal.PhoneNumber.UserName} " +
                    $"and {terminal.Port.ConnectedCallPort.Terminal.PhoneNumber.UserName} are talking...");
            }
            else
            {
                Console.WriteLine($"{terminal.PhoneNumber.UserName} has no call to answer.");
            }
        }

        public static void OnDrop(object sender, TerminalEventArgs e)
        {
            var terminal = sender as ITerminal;
            if (terminal == null)
            {
                return;
            }
            if (terminal.Port.PortState == Port.PortState.Busy)
            {
                Console.WriteLine($"{terminal.PhoneNumber.UserName} has ended the call with {terminal.Port.ConnectedCallPort.Terminal.PhoneNumber.UserName}.");
                terminal.Port.DropConnection();
            }
            else
            {
                Console.WriteLine($"{terminal.PhoneNumber.UserName} has no call to drop.");
            }
        }

        public static void OnUnableToCall(object sender, TerminalEventArgs e)
        {
            var terminal = sender as ITerminal;
            if (terminal == null)
            {
                return;
            }
            Console.WriteLine($"{e.SomeonesNumber}");
        }

    }
}
