using System.Text;
using MicroServices;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;

namespace CommandTreatment;

public class MQReicever : MQInteractor
{
    protected Random random;
    public event Action<Command> OnCommandReceived;
    public event Action<string> OnTextReceived;

    public async void StartWaitingForMessages()
    {
        while (!Ready)
        {
            Console.WriteLine("Waiting for RabbitMQ to be ready...");
            await Task.Delay(500);
        }

        var decl = await channel.QueueDeclareAsync(QUEUENAME + (DateTime.Now.Microsecond).ToString(), false, true, true);
        String queueName = decl.QueueName;
        Console.WriteLine($"Created queue {queueName}");
        await channel.QueueBindAsync(queueName, EXCHANGENAME, "");
        Console.WriteLine($"Bound queue {queueName} to exchange {EXCHANGENAME}");
        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += (model, ea) =>
        {
            var message = Encoding.UTF8.GetString(ea.Body.ToArray());
            //Console.WriteLine($" [x] Received {message}");
            Command cmd = JsonConvert.DeserializeObject<Command>(message);
            if (!cmd.Equals(default(Command)))
                OnCommandReceived?.Invoke(cmd);
            OnTextReceived?.Invoke(message);
            return Task.CompletedTask;
        };
        Console.WriteLine("Created consumer and added callback");
        await channel.BasicConsumeAsync(queueName, autoAck: true, consumer: consumer, cancellationToken: default,
            consumerTag: "", noLocal: false, exclusive: false, arguments: null);
        Console.WriteLine("Started consuming");
    }
}