using BLL;

namespace Presentation
{
    public class RabbitMqHostedService : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public RabbitMqHostedService(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var messageConsumer = scope.ServiceProvider.GetRequiredService<IMessageConsumer>();
            await Task.Factory.StartNew(() =>
            {
                messageConsumer.StartConsuming();
            }, stoppingToken);
        }
    }
}
}
