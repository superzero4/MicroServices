using CommandTreatment;
using MicroServices;
using Newtonsoft.Json;

namespace TextProcessor;


class Program
{
    static async Task Main(string[] args)
    {
        MQReicever ct = new MQReicever();
        ct.StartWaitingForMessages();
        ct.OnTextReceived += (string message) =>
        {
            TextCommand tc = JsonConvert.DeserializeObject<TextCommand>(message);
            if(!tc.Equals(default(TextCommand)))
            {
                Console.WriteLine(tc.fullOperation);
            }
            else
            {
                //Ignore all other commands/message that are not json of MathCommands
            }
        };
        await Task.Delay(Timeout.Infinite);
    }
}