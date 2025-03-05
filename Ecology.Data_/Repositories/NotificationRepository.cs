using Ecology.Data.DataLayerModels;
using Ecology.Data.Interface.Repositories;
using Ecology.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecology.Data.Repositories
{
    public interface INotificationRepositoryReal : INotificationRepository<NotificationData>
    {
        List<NotificationTextAndId> GetUnseenMessages(int userId);
        void MessageWasSeen(int id, int userId);
        int RemoveOutdatedNotifications();
    }

    public class NotificationRepository
        : BaseRepository<NotificationData>, INotificationRepositoryReal
    {
        public NotificationRepository(WebDbContext webDbContext) : base(webDbContext)
        {
        }

        public List<NotificationTextAndId> GetUnseenMessages(int userId)
        {
            var now = DateTime.UtcNow;
            return _dbSet
                .Where(notification =>
                    !notification
                        .UsersWhoAlreadySawIt
                        .Any(user => user.Id == userId)
                    && (notification.Start == null || notification.Start < now)
                    && (notification.End == null || notification.End > now))
                .Select(x => new NotificationTextAndId
                {
                    Id = x.Id,
                    Text = x.Text,
                })
                .ToList();
        }

        public void MessageWasSeen(int id, int userId)
        {
            var notification = _dbSet
                .Include(x => x.UsersWhoAlreadySawIt)
                .First(x => x.Id == id);

            var user = _webDbContext.Users.First(x => x.Id == userId);

            notification.UsersWhoAlreadySawIt.Add(user);

            _webDbContext.SaveChanges();
        }
        
        /// <summary>
        /// Remove notifications which was end date yesterday or later
        /// </summary>
        /// <returns></returns>
        public int RemoveOutdatedNotifications()
        {
            var yesterday = DateTime.UtcNow.AddDays(-1);
            var notificationsForRemove = _dbSet
                .Where(x => x.End != null && x.End < yesterday)
                .ToList();
            
            foreach (var notification in notificationsForRemove)
            {
                notification.Start = notification.Start?.ToUniversalTime(); // Преобразуем в UTC
                notification.End = notification.End?.ToUniversalTime(); // Преобразуем в UTC
            }
            
            _dbSet.RemoveRange(notificationsForRemove);

            _webDbContext.SaveChanges();

            return notificationsForRemove.Count();
        }
    }
}
