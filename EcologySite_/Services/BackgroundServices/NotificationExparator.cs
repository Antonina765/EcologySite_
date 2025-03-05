using Ecology.Data.Repositories;
using System.ComponentModel;

namespace EcologySite.Services.BackgroundServices
{
    public class NotificationExparator : BackgroundService
    {
        private IServiceProvider _diContainer;

        public NotificationExparator(IServiceProvider diContainer)
        {
            _diContainer = diContainer;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(ExpireNotification);
        }

        private async void ExpireNotification()
        {
            while (true)
            {
                using (var di = _diContainer.CreateScope())
                {
                    var repository = di
                        .ServiceProvider
                        .GetRequiredService<INotificationRepositoryReal>();

                    var countOfRemovedNotifications = repository.RemoveOutdatedNotifications();
                    Console.WriteLine($"Count Of Removed Notifications: {countOfRemovedNotifications}");
                }

                await Task.Delay(5 * 1000);
            }
        }
    }
}
