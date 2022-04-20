

using EventBus.EventBus.Abstractions;

namespace Consumer.Service;

public class ConsumerService
{
    private readonly IEventBus _eventBus;

    public ConsumerService(IEventBus eventBus)
    {

    }
}