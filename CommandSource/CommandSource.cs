using System.Text;
using MicroServices;
using System.Diagnostics;
using Newtonsoft.Json;
using RabbitMQ.Client.Exceptions;

using RabbitMQ.Client;

namespace CommandSource;


public class CommandSource : MQInteractor
{
    public void IssueNewCommand(int commandCode, string commandData)
    {
       BasicPublish(JsonConvert.SerializeObject(new Command(){Code = commandCode, Message = new Message(){Text = commandData}, Data = new Data(){Name = "CommandSource", Description = "Command issued", Version = "1.0.0"}}));
    }
    
}