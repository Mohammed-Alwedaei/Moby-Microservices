using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moby.ServiceBus;
public interface IMessageBusManager
{
    Task PublishMessage(BaseMessageModel baseMessage, string topicName, string connectionString);
}
