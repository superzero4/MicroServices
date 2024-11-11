using MicroServices;

namespace CommandSource;

class Program
{
    static string[] commands = new string[]
    {
        "math add 2 3",
        "text up HeLlO",
        "math sub 10 3",
        "text low HeLlO",
        "math mult 2 3",
        "text rev Hello World where we live in",
    };
    static async Task<int> Main(string[] args)
    {
        CommandSource cs = new CommandSource();
        int i = 0;
        await Task.Delay(2500);
        Console.WriteLine("Enter your commands or type 'exit' to quit : ");
        while (true)
        {
            //cs.IssueNewCommand(i++,"Sample command");
            if (i<commands.Length)
            {
                Console.WriteLine($"Automatic : {commands[i]}");
                cs.IssueNewCommand(i, commands[i]);
                i++;
                await Task.Delay(300);
                continue;
            }
            var line = Console.ReadLine();
            if(line.ToLower() == "exit")
                return 1;
            cs.IssueNewCommand(i, line);
                
            i++;
        }

        return 0;
    }
}