using RabbitMQ.Client;

public class RabbitMqChannelFactory
{
    private readonly IConnection _connection;

    public RabbitMqChannelFactory(IConnection connection)
    {
        _connection = connection;
    }

    public IModel CreateChannel()
    {
        return _connection.CreateModel();
    }
}
