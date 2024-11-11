using MicroServices;
using Newtonsoft.Json;

namespace CommandTreatment;

class Program
{
    private static bool oneMessage = false;

    static async Task Main(string[] args)
    {
        MQReicever ct = new MQReicever();
        Console.BackgroundColor = ConsoleColor.Black;
        ct.StartWaitingForMessages();
        ct.OnCommandReceived += (Command message) =>
        {
            bool val = ValidateCommand(message.Message.Text, out string outJson);
            if (val)
            {
                ct.BasicPublish(outJson);
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            Console.WriteLine($"Command {message.Code} in validation process : " + (val ? "PASSED " : "FAILED"));
        };
        await Task.Delay(Timeout.Infinite);
    }

    private static bool ValidateCommand(string input, out string outJson)
    {
        var split = input.Split(' ');
        outJson = "";
        if (split[0].ToLower() == "math")
        {
            if (split.Length != 4)
                return false;
            int index = MathCommand.Commands.Select(s=>s.ToLower()).ToList().IndexOf(split[1].ToLower());
            if (index < 0)
                return false;
            //Console.WriteLine("Validating command: " + input + " splitted : " + split.Length +" "+ split[1] + " " + split[2] + " " + split[3]);
            if (!int.TryParse(split[2], out int x1) || !int.TryParse(split[3], out int x2))
                return false;
            MathCommand mc = new MathCommand(index, x1, x2);
            outJson = JsonConvert.SerializeObject(mc);
            return true;
        }

        if (split[0].ToLower() == "text")
        {
            if (split.Length < 3)
                return false;
            int index = TextCommand.Commands.Select(s=>s.ToLower()).ToList().IndexOf(split[1].ToLower());
            if (index < 0)
                return false;
            TextCommand tc = new TextCommand(index, string.Join(' ', split.Skip(2)));
            outJson = JsonConvert.SerializeObject(tc);
            return true;
        }

        return false;
    }
}