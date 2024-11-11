using System.Text;
using Newtonsoft.Json;

namespace MicroServices;

[System.Serializable]
public struct TextCommand
{
    public enum CommandType
    {
        Up = 0,
        Low = 1,
        Rev = 2
    }

    public static string[] Commands => Enum.GetNames<CommandType>();

    public TextCommand(int type, string text)
    {
        this.type = (CommandType)type;
        this.text = text;
    }

    public CommandType type;
    public string text;
    public string fullOperation => text + " {" + operation + "} => " + result;
    public string operation => type switch
    {
        CommandType.Up => "to upper case",
        CommandType.Low => "to lower case",
        CommandType.Rev => "reversed",
        _ => ""
    };
    public string result => type switch
    {
        CommandType.Up => text.ToUpper(),
        CommandType.Low => text.ToLower(),
        CommandType.Rev => new string(text.Reverse().ToArray()),
        _ => ""
    };
}

[System.Serializable]
public struct MathCommand
{
    public enum CommandType
    {
        Add = 0,
        Sub = 1,
        Mult = 2,
        Div = 3
    }

    public static string[] Commands => Enum.GetNames<CommandType>();

    public MathCommand(int type, int a, int b)
    {
        this.type = (CommandType)type;
        this.a = a;
        this.b = b;
    }

    public CommandType type;
    public int a;
    public int b;
    public string Equation => $"{a} {op()} {b} = {result}";

    private string op()
    {
        switch (type)
        {
            case CommandType.Add:
                return "+";
            case CommandType.Sub:
                return "-";
            case CommandType.Mult:
                return "*";
            case CommandType.Div:
                return "/";
            default:
                return "";
        }
    }

    public float result => type switch
    {
        CommandType.Add => a + b,
        CommandType.Sub => a - b,
        CommandType.Mult => a * b,
        CommandType.Div => a / b,
        _ => 0
    };
}

[System.Serializable]
public struct Command
{
    public int Code;
    public Message Message;
    public Data Data;

    public static Command FromJson(byte[] json)
    {
        return FromJson(Encoding.UTF8.GetString(json));
    }

    public static Command FromJson(string json)
    {
        try
        {
            return JsonConvert.DeserializeObject<Command>(json);
        }
        catch (JsonException)
        {
            return new Command()
            {
                Code = -1, Message = new Message() { Text = "Invalid JSON" }, Data = new Data() { Description = json }
            };
        }
    }
}

[Serializable]
public struct Message
{
    public string Text;
}

[Serializable]
public struct Data
{
    public string Name = "MicroServices";
    public string Version = "1.0.0";
    public string Description = "A simple microservices framework for .NET 6";

    public Data(string name, string version, string description)
    {
        Name = name;
        Version = version;
        Description = description;
    }
}