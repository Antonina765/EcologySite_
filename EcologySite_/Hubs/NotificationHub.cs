using Microsoft.AspNetCore.SignalR;

namespace EcologySite.Hubs
{
    public interface INotificationHub
    {
        Task NewNotification(string message);
    }

    public class NotificationHub : Hub<INotificationHub>
    {
    }
}
