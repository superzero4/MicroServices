using System.Drawing;
using CommandTreatment;
using MicroServices;

namespace CommandMonitor;

class Program
{
    static async Task Main(string[] args)
    {
        Random rd = new Random();
        var colors = Enum.GetValues<ConsoleColor>().Cast<ConsoleColor>();
        MQReicever ct = new MQReicever();
        ct.StartWaitingForMessages();
        ct.OnTextReceived += (string message) =>
        {
            if (!string.IsNullOrEmpty(message))
            {
                Console.ForegroundColor = colors.ElementAt(rd.Next(colors.Count()));
                Console.WriteLine($"Command monitor: {message}");
            }
        };
        await Task.Delay(Timeout.Infinite);
    }
}