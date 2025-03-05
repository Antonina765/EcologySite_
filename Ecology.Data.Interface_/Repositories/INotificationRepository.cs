using Ecology.Data.Interface.Models;

namespace Ecology.Data.Interface.Repositories
{
    public interface INotificationRepository<T> : IBaseRepository<T>
        where T : INotificationData
    {
    }
}
