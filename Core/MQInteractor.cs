using System.Diagnostics;
using MicroServices;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace MicroServices;

using RabbitMQ.Client;
using System.Text;

public class MQInteractor
{
    protected const string QUEUENAME="queue";
    protected const string EXCHANGENAME="commands";
    private ConnectionFactory factory;
    private IConnection connection;
    protected IChannel channel { get; private set; }
    private static Process docker;
    public bool Ready { get; private set; }
    private bool IsRabbitMQRunning()
    {
        try
        {
            using (var client = new System.Net.Http.HttpClient())
            {
                var response = client.GetAsync("http://localhost:15672").Result;
                return response.IsSuccessStatusCode;
            }
        }
        catch
        {
            return false;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="s"></param>
    /// <param name="exchange">empty string to normal broadcast</param>
    public async void BasicPublish(string s,string exchange=EXCHANGENAME)
    {
        while(!Ready)
        {
            await Task.Delay(500);
        }
        byte[] body = Encoding.UTF8.GetBytes(s);

        await channel.BasicPublishAsync(exchange: exchange, routingKey: "", body: body);
        //Console.WriteLine($"[Sender] Sent {s}");
    }
    public async void Connect()
    {
        int tryouts = 0;
        while(!IsRabbitMQRunning())
        {
            Console.WriteLine("Waiting for local server to respond...");
            await Task.Delay(500);
            tryouts++;
            if (tryouts > 20)
            {
                Console.WriteLine("Docker is not responding. Please start the docker service and try again.");
            }
        }
        factory = new ConnectionFactory() { HostName = "localhost" };
        try
        {
            connection = await factory.CreateConnectionAsync();
        }
        catch (BrokerUnreachableException e)
        {
            Console.WriteLine("RabbitMQ server is not running. Please start the server and try again.");
            Environment.Exit(1);
        }
        channel = await connection.CreateChannelAsync();
        await channel.ExchangeDeclareAsync(EXCHANGENAME, ExchangeType.Fanout);
        //Enough for publishing
        Ready = true;
        Console.WriteLine("Connected to RabbitMQ server");
    }
    public MQInteractor()
    {
        Ready = false;
        if(!IsRabbitMQRunning())
        {
            docker = new Process();
            docker.StartInfo.UseShellExecute = true;
            docker.StartInfo.CreateNoWindow = true;
            docker.StartInfo.FileName = "docker";
            docker.StartInfo.Arguments = "run -it --rm --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:4.0-management";
            docker.Start();
            Console.WriteLine("Starting docker and RabbitMQ server");
        }
        Connect();
    }

    ~MQInteractor()
    {
        channel.Dispose();
        connection.Dispose();
        if(docker!=null && !docker.HasExited)
        {
            Console.WriteLine("Closing docker");
            docker.Kill();
            docker = null;
        }
    }
}