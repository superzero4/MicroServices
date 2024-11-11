using CommandTreatment;
using MicroServices;
using Newtonsoft.Json;

namespace MathProcessor;


class Program
{
    static async Task Main(string[] args)
    {
        MQReicever ct = new MQReicever();
        ct.StartWaitingForMessages();
        ct.OnTextReceived += (string message) =>
        {
            MathCommand mc = JsonConvert.DeserializeObject<MathCommand>(message);
            if(!mc.Equals(default(MathCommand)))
            {
                Console.WriteLine(mc.Equation);
            }
            else
            {
                //Ignore all other commands/message that are not json of MathCommands
            }
        };
        await Task.Delay(Timeout.Infinite);
    }
}