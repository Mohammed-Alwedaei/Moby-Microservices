namespace Moby.ServiceBus;
public class BaseMessageModel
{
    public int Id { get; set; }

    public DateTime Timestamp { get; set; } = DateTime.Now;
}
